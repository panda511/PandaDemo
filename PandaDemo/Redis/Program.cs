using Newtonsoft.Json;
using RedisDemo;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    class Program
    {
        static void Main66(string[] args)
        {
            Person p = new Person();
            p.Id = 1;
            p.Name = "李四2";
            p.Age = 22;
            List<Person> list = new List<Person>();
            list.Add(p);

            ConfigurationOptions opt = ConfigurationOptions.Parse("127.0.0.1:63790");
            opt.Password = "666";
            opt.AllowAdmin = true;
            opt.ConnectTimeout = 5000;
            opt.SyncTimeout = 5000;
            opt.DefaultDatabase = 5;

            try
            {
                ConnectionMultiplexer c = ConnectionMultiplexer.Connect(opt); //"127.0.0.1:6379,allowadmin=true"
                IDatabase db = c.GetDatabase();

                db.StringSet("abc2:name", JsonConvert.SerializeObject(list), TimeSpan.FromMinutes(5)); //设置过期时间

                List<Person> list2 = JsonConvert.DeserializeObject<List<Person>>(db.StringGet("abc2:name"));

                Console.WriteLine(list2[0].Name);
            }
            catch
            {
                Console.WriteLine("error");
            }
            Console.Read();
        }

        static void Main22(string[] args)
        {
            RedisHelper h = new RedisHelper();
            //h.StringSet("name", "tom");
            string s = h.StringGet("name");
            Console.WriteLine(s);
        }

        static void Main(string[] args)
        {
            var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379,allowAdmin = true");
            redis.GetDatabase().ScriptEvaluate(LuaScript.Prepare(
                //Redis的keys模糊查询：
                " local ks = redis.call('KEYS', @keypattern) " + //local ks为定义一个局部变量，其中用于存储获取到的keys
                " for i=1,#ks,5000 do " +    //#ks为ks集合的个数, 语句的意思： for(int i = 1; i <= ks.Count; i+=5000)
                "     redis.call('del', unpack(ks, i, math.min(i+4999, #ks))) " + //Lua集合索引值从1为起始，unpack为解包，获取ks集合中的数据，每次5000，然后执行删除
                " end " +
                " return true "
                ),
                new { keypattern = "mykey*" }
            );

            //redis0:6379,redis1:6380,keepAlive=180,version=2.8.8,$CLIENT=,$CLUSTER=,$CONFIG=,$ECHO=,$INFO=,$PING=
            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints = {
                    { "redis0", 6379 },
                    { "redis1", 6380 }
                },
                CommandMap = CommandMap.Create(
                    // 排除几个命令
                    new HashSet<string> { "INFO", "CONFIG", "CLUSTER", "PING", "ECHO", "CLIENT" }, available: false
                ),
                KeepAlive = 180,
                DefaultVersion = new Version(2, 8, 8),
                Password = "changeme"
            };

            string someKey = redis.GetDatabase().KeyRandom();
            redis.GetDatabase().KeyDelete("key");

            //redis.GetDatabase().ke

            // 取得目标服务器
            var server = redis.GetServer("someServer");

            // 在索引为0的数据库中展示出所有的key，这个key的名字必须匹配 *foo*
            foreach (var key in server.Keys(pattern: "*foo*"))
            {
                Console.WriteLine(key);
            }

            // 从索引为0的数据库中清除所有的key
            server.FlushDatabase();
        }

        /// <summary>
        /// 移除以指定key开头的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void RemovePatten(string key)
        {
            //Redis的keys模糊查询： local ks为定义一个局部变量，其中用于存储获取到的keys
            //#ks为ks集合的个数, 语句的意思： for(int i = 1; i <= ks.Count; i+=5000)
            //Lua集合索引值从1为起始，unpack为解包，获取ks集合中的数据，每次5000，然后执行删除
            string script = @"
                local ks = redis.call('KEYS', @keypattern)
                for i=1,#ks,5000 do
                redis.call('del', unpack(ks, i, math.min(i+4999, #ks))) 
                end
                return true
            ";
            var redis = ConnectionMultiplexer.Connect("127.0.0.1:6379,allowAdmin = true");
            redis.GetDatabase().ScriptEvaluate(LuaScript.Prepare(script), new { keypattern = key + @"*" });
        }

        public class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
