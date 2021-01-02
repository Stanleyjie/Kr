using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class BaseService<T> : IBaseService<T> where T : class,new()
    {
        protected IBaseRepository<T> _baseRepository { get; set; }
 
        //不使用构造函数
        public bool Delete(T entity)
        {
            return _baseRepository.Delete(entity);
        }

        public bool Delete(Expression<Func<T, bool>> where)
        {
            return _baseRepository.Delete(where);
        }

        public bool DeleteById(object id)
        {
            return _baseRepository.DeleteById(id);
        }

        public bool DeleteByIds(object[] ids)
        {
            return _baseRepository.DeleteByIds(ids);
        }
         
        public List<TEntity> ExecuteSelectQuery<TEntity>(string sql, object pars)
        {
            return _baseRepository.ExecuteSelectQuery<TEntity>(sql, pars);
        }

        public int ExecuteSqlExecuteSqlCommand(string sql, object pars)
        {
            return _baseRepository.ExecuteSqlExecuteSqlCommand(sql, pars);
        }

        public IEnumerable<T> FindAll()
        {
            return _baseRepository.FindAll();
        }

        public T FindByClause(Expression<Func<T, bool>> predicate)
        {
            return _baseRepository.FindByClause(predicate);
        }

        public T FindById(object pkValue)
        {
            return _baseRepository.FindById(pkValue);
        }

        public IEnumerable<T> FindListByClause(Expression<Func<T, bool>> predicate, string orderBy = "")
        {
            return _baseRepository.FindListByClause(predicate, orderBy);
        }

        public List<T> FindPagedList(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20)
        {
            return _baseRepository.FindPagedList(predicate, orderBy, pageIndex, pageSize);
        }

        public T Insert(T entity)
        {
            return _baseRepository.Insert(entity);
        }

        public bool Update(T entity)
        {
            return _baseRepository.Update(entity);
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="operate"></param>
        /// <returns></returns>
        public bool Submit(Action operate)
        {
 
                try
                {
                    _baseRepository.BeginTran();
                    operate();
                    _baseRepository.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    _baseRepository.RollbackTran();
                    //任务出差 就不会提交 自动回滚
                    throw ex;
                }
 
        }

    }
}
