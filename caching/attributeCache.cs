using ado.Models;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ado.caching
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class attributeCache : ActionFilterAttribute
    {
        public string CacheKey { get; set; }
       
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IDatabase cache = stackExchange.Connection.GetDatabase();
            
            List<RedisValue> jsonStrings = cache.ListRange(CacheKey).ToList();
            if (jsonStrings.Count<=0)
            {
                // Nếu dữ liệu không tồn tại trong cache, chạy đến action result
                base.OnActionExecuting(filterContext); 
            }
            else
            {
              


            }
        }
       
       

    }
}