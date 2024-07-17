using ado.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Documents;

namespace ado.caching
{
    public class test11<T>
    {
        private readonly IDatabase _database;
       
        public test11()
        {
            _database = stackExchange.Connection.GetDatabase();
           
        }

        public void AddToList(string key, List<T> data,TimeSpan x)
        {
            
            var serializedData = JsonConvert.SerializeObject(data);
            _database.ListRightPush(key, serializedData);
            _database.KeyExpire(key, x);
            
        }

        public List<T> ReadDataFromRedis(string cacheKey)
        {
            IDatabase cache = _database;
            List<RedisValue> jsonStrings = cache.ListRange(cacheKey).ToList();

            List<T> myObjects = new List<T>();
            foreach (var jsonString in jsonStrings)
            {
                // Kiểm tra nếu chuỗi JSON là một mảng hay không
                if (jsonString.ToString().StartsWith("["))
                {
                    // Nếu là mảng JSON, deserializing thành danh sách đối tượng
                    List<T> arrayObjects = JsonConvert.DeserializeObject<List<T>>(jsonString);
                    myObjects.AddRange(arrayObjects);
                }
                else
                {
                    // Nếu không phải mảng JSON, deserializing thành đối tượng đơn
                    T obj = JsonConvert.DeserializeObject<T>(jsonString);
                    myObjects.Add(obj);
                }
            }

            return myObjects;
        }
        public bool CheckKey(string key)
        {
            IDatabase cache = _database;

            List<RedisValue> jsonStrings = cache.ListRange(key).ToList();
            return jsonStrings.Count > 0;
        }
    }
}