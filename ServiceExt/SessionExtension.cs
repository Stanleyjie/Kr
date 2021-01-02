using System;
using System.Collections.Generic;
using System.Text;
//using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ServiceExt
{
    public static class SessionExtension
    {
        ///// <summary>
        ///// 设置Session对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="session"></param>
        ///// <param name="key"></param>
        ///// <param name="obj"></param>
        //public static void SetObject<T>(this ISession session, string key, T obj)
        //{
        //    session.SetObject(key, obj);
        //}

        ///// <summary>
        ///// 获取Session对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="session"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static T GetObject<T>(this ISession session, string key)
        //{
        //    T result = default(T);
        //    var value = session.GetString(key);
        //    if (!String.IsNullOrEmpty(value))
        //    {
        //        result = JsonConvert.DeserializeObject<T>(value);
        //    }
        //    return result;
        //}

    }
}
