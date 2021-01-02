using Repository.IRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        protected SqlSugarClient _db;

        public BaseRepository()
        {
            _db = DbContextFactory.GetInstance();
        }

        #region 基本的数据库操作

        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public T FindById(object pkValue)
        {
            var entity = _db.Queryable<T>().InSingle(pkValue);
            return entity;
        }

        /// <summary>
        /// 查询所有数据(无分页,请慎用)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> FindAll()
        {
            var list = _db.Queryable<T>().ToList();
            return list;
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序</param>
        /// <returns>泛型实体集合</returns>
        public IEnumerable<T> FindListByClause(Expression<Func<T, bool>> predicate, string orderBy = "")
        {
            var query = _db.Queryable<T>().Where(predicate);
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }
            var entities = query.ToList();
            return entities;
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <returns></returns>
        public T FindByClause(Expression<Func<T, bool>> predicate)
        {
            var entity = _db.Queryable<T>().First(predicate);
            return entity;
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public T Insert(T entity)
        {
            //返回插入数据的模型
            var _entity = _db.Insertable(entity).ExecuteReturnEntity();

            return _entity;
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            //这种方式会以主键为条件
            var i = _db.Updateable(entity).ExecuteCommand();
            return i > 0;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            var i = _db.Deleteable(entity).ExecuteCommand();
            return i > 0;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> @where)
        {
            var i = _db.Deleteable<T>(@where).ExecuteCommand();
            return i > 0;
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(object id)
        {
            var i = _db.Deleteable<T>(id).ExecuteCommand();
            return i > 0;
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(object[] ids)
        {
            var i = _db.Deleteable<T>().In(ids).ExecuteCommand();
            return i > 0;
        }

        /// <summary>
        /// 根据条件查询分页数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        public List<T> FindPagedList(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20)
        {
            var totalCount = 0;
            var list = _db.Queryable<T>().OrderBy(orderBy).ToPageList(pageIndex, pageSize, ref totalCount);
            return list;
        }

        #endregion 基本的数据库操作

        /// <summary>
        /// list转table
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        private DataTable ListToDataTable<Entity>(List<Entity> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return new DataTable();
            }

            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            //PropertyInfo[] entityProperties = entityType.GetProperties();
            var entityProperties = entityType.GetProperties();
            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable("dt");
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }

            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="list"></param>
        /// <param name="tableName"></param>
        public void BulkCopy<Entity>(List<Entity> list, string tableName)
        {
            SqlConnection conn = _db.Ado.Connection as SqlConnection;
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                DataTable dt = ListToDataTable(list);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dt);
            }
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public List<TEntity> ExecuteSelectQuery<TEntity>(string sql, object pars)
        {
            return _db.Ado.SqlQuery<TEntity>(sql, pars).ToList();
        }

        /// <summary>
        /// 执行sql 的存储过程
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <returns></returns>
        public int ExecuteSqlExecuteSqlCommand(string sql, object pars)
        {
            return pars == null ? _db.Ado.ExecuteCommand(sql) : _db.Ado.ExecuteCommand(sql, pars);
        }

        public dynamic ExecuteSelectQueryAny(string sql, object pars)
        {
            return _db.Ado.SqlQuery<dynamic>(sql, pars);
        }

        public void BeginTran()
        {
            _db.Ado.BeginTran();
        }

        public void CommitTran()
        {
            _db.Ado.CommitTran();
        }

        public void RollbackTran()
        {
            _db.Ado.RollbackTran();
        }
    }
}