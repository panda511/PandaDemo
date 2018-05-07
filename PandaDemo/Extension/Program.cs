using Extension.Extention;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    class Program
    {
        static void Main(string[] args)
        {
            //string str = "admin1801235";
            //Console.WriteLine(str.ToMd5());

            JObject jObject = new JObject();
            jObject.Add(new JProperty("Name", "James"));
            jObject.Add(new JProperty("Age", "11"));

            string json = jObject.ToString();
            var p = json.ToObject<Person>();

            //Console.WriteLine(json);


            string test = "000and1235and859and777";

            Console.WriteLine(test.ReplaceFirst("and", ""));


            Console.Read();
        }



    }

    class Person
    {
        public string NaMe { get; set; }
        public int Age { get; set; }
    }
}
