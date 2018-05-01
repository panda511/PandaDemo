using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //原生查询
                //{"CreateDate":{"$gte":new Date("2017-02-26T09:00:00Z")}}

                //var client2 = new MongoClient(new MongoClientSettings
                //{
                //    Server = new MongoServerAddress("127.0.0.1"),
                //    ServerSelectionTimeout = TimeSpan.FromSeconds(1),
                //});

                string connectionString = "mongodb://127.0.0.1:27017?ServerSelectionTimeout=1";
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("greenfamily");
                var collection = database.GetCollection<ExceptionLog>("OperationInfoLog");

                //单个查询
                var find1 = collection.Find(item => item.FunctionName == "abcd").FirstOrDefault();

                //集合查询
                var list = collection.Find(item => item.CreateDate > DateTime.Now).ToList();

                //分页查询
                var list2 = collection.AsQueryable().Where(c => c.CreateDate > DateTime.Now).Skip(10).Take(10).ToList();

                //删除
                var result = collection.DeleteMany(c => c.CreateDate < DateTime.Parse("2017-7-30 00:00:00"));
                long count = result.DeletedCount;

            }
            catch (Exception ex)
            {
            }
        }
    }

    public class ExceptionLog
    {
        public ObjectId Id { get; set; }
        public string guid { get; set; }
        public string ExceptionContent { get; set; }
        public string FunctionName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
