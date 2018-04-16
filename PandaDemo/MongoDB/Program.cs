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
                string connectionString = "mongodb://127.0.0.1:27017?ServerSelectionTimeout=1";

                //var client2 = new MongoClient(new MongoClientSettings
                //{
                //    Server = new MongoServerAddress("127.0.0.1"),
                //    ServerSelectionTimeout = TimeSpan.FromSeconds(1),
                //});

                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("greenfamily");
                var collection = database.GetCollection<ExceptionLog>("OperationInfoLog");

                //var find1 = collection.Find(item => item.Name == "北京").FirstOrDefault();

                var result = collection.DeleteMany(c => c.CreateDate < DateTime.Parse("2017-7-30 00:00:00"));

                //var list = collection.Find(item => item.CreateDate > DateTime.Now.AddDays(-1)).ToList();

                //分页
                //var list2 = collection.AsQueryable().Where(c => c.CreateDate > DateTime.Now.AddDays(-2)).Skip(10).Take(10).ToList();

                //richTextBox1.Text = list2.ToJson();
                //count = list2.Count;


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
