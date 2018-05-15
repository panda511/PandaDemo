using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperSocket.WebSocket;

namespace WebSuperSocketServer
{
    /// <summary>
    /// 身份认证过滤器
    /// </summary>
    public class AuthorizeFilter : IConnectionFilter
    {
        private bool IsAuthenticated = false;

        public string Name { get; private set; }

        public bool Initialize(string name, IAppServer appServer)
        {
            Name = name;

            return true;
        }

        public bool AllowConnect(IPEndPoint remoteAddress)
        {
            return IsAuthenticated;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var server = new MyWebSocketServer();

            server.NewSessionConnected += ServerNewSessionConnected;
            server.NewMessageReceived += ServerNewMessageRecevied;
            server.SessionClosed += ServerSessionClosed;

            server.NewDataReceived += (session, bytes) =>
            {
                //接收到新的二进制消息
            };

            Timer timer = new Timer((data) =>
            {
                //对当前已连接的符合条件的会话进行广播
                var sessions = server.GetSessions(c => c.MyCustomerId == "1");
                if (sessions != null)
                {
                    foreach (var session in sessions)
                    {
                        session.Send(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }

            }, null, 10000, 10000);

            try
            {
                var listenerConfig1 = new ListenerConfig()
                {
                    Ip = "127.0.0.1",
                    Port = 2012
                };

                var listenerConfig2 = new ListenerConfig()
                {
                    Ip = "127.0.0.1",
                    Port = 4142
                };

                var list = new List<ListenerConfig>
                {
                    listenerConfig1,
                    listenerConfig2
                };

                var serverConfig = new ServerConfig
                {
                    Ip = "127.0.0.1",
                    Port = 2012,
                    Name = "DyWebSocketServer8",
                    //Listeners = list,
                    MaxConnectionNumber = 311 //最大连接数，默认100
                };

                var filters = new List<AuthorizeFilter>() { new AuthorizeFilter() };

                bool s1 = server.Setup(serverConfig, connectionFilters: filters);
                bool s2 = server.Start(); //启动侦听
                //bool s1 = server.Setup("127.0.0.1", 4141); //对要侦听的IP及端口进行设置
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常:" + ex.Message);
            }
            Console.ReadKey();

            server.Stop();
        }

        static int i = 1;

        public static void ServerNewSessionConnected(MyWebSocketSession session)
        {
            //发起一个连接之前验证

            Console.WriteLine("有新的连接:" + (i++));

            Console.WriteLine(session.Host);
            Console.WriteLine(session.Path);
            Console.WriteLine("--------------------------");

            string name = "";
            string password = "";

            //解析客户端传过来的参数
            var arr = session.Path.Split('?');
            if (arr.Length > 1)
            {
                string ps = arr[1];
                var array = ps.Split('&');
                foreach (string item in array)
                {
                    string key = item.Split('=')[0];
                    string value = item.Split('=')[1];
                    if (key == "name")
                    {
                        name = value;
                    }
                    else if (key == "password")
                    {
                        password = value;
                    }
                }
            }

            //WebSocket 令牌机制
            //1. 服务器端为每个 WebSocket 客户端生成唯一的一次性 Token；
            //2. 客户端将 Token 作为 WebSocket 连接 URL 的参数（譬如 ws://echo.websocket.org/?token＝randomOneTimeToken），发送到服务器端进行 WebSocket 握手连接；
            //3. 服务器端验证 Token 是否正确，一旦正确则将这个 Token 标示为废弃不再重用，同时确认 WebSocket 握手连接成功；如果 Token 验证失败或者身份认证失败，则返回 403 错误。


            //检查Origin头
            if (session.Origin != "aaaaa")
            {
            }

            //单IP可建立连接的最大连接数    

            //验证身份
            if (name != "admin" || password != "123456")
            {
                //非法身份直接断开连接
                //session.Close((CloseReason)1);
                //session.Close(CloseReason.InternalError);
                //session.CloseWithHandshake(403, "非法身份");
            }
        }

        public static void ServerNewMessageRecevied(MyWebSocketSession session, string value)
        {
            Console.WriteLine("接收到新的文本消息:" + value);
            session.Send("当前SessionID：" + session.SessionID);

            session.MyCustomerId = value;
        }


        private static void ServerSessionClosed(MyWebSocketSession session, CloseReason value)
        {
            Console.WriteLine("有断开的连接:" + value.ToString() + "     " + session.MyCustomerId);
        }

    }

    class MyWebSocketSession : WebSocketSession<MyWebSocketSession>
    {
        public string MyCustomerId { get; internal set; }
    }

    class MyWebSocketServer : WebSocketServer<MyWebSocketSession>
    {

    }
}


/// <summary>
/// 配置文件启动
/// </summary>
namespace ConsoleApp2
{
    class Program 
    {
        static void Main999(string[] args)
        {
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();

            var bootstrap = BootstrapFactory.CreateBootstrap();

            if (!bootstrap.Initialize())
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadKey();
                return;
            }
            //SuperSocket.WebSocket.WebSocketServer
            var result = bootstrap.Start();

            Console.WriteLine("Start result: {0}!", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            Console.WriteLine();

            //Stop the appServer
            bootstrap.Stop();

            Console.WriteLine();
            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }
    }

    public class IPConnectionFilter : IConnectionFilter
    {
        public string Name { get; private set; }

        public bool Initialize(string name, IAppServer appServer)
        {
            Name = name;

            //var ipRange = appServer.Config.Options.GetValue("ipRange");

            //string[] ipRangeArray;

            //if (string.IsNullOrEmpty(ipRange) || (ipRangeArray = ipRange.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)).Length <= 0)
            //{
            //    throw new ArgumentException("The ipRange doesn't exist in configuration!");
            //}

            //m_IpRanges = new Tuple<long, long>[ipRangeArray.Length];

            //for (int i = 0; i < ipRangeArray.Length; i++)
            //{
            //    var range = ipRangeArray[i];
            //    m_IpRanges[i] = GenerateIpRange(range);
            //}

            return true;
        }

        public bool AllowConnect(IPEndPoint remoteAddress)
        {
            //var ip = remoteAddress.Address.ToString();
            //var ipValue = ConvertIpToLong(ip);

            //for (var i = 0; i < m_IpRanges.Length; i++)
            //{
            //    var range = m_IpRanges[i];

            //    if (ipValue > range.Item2)
            //        return false;

            //    if (ipValue < range.Item1)
            //        return false;
            //}

            return false;
        }





        private Tuple<long, long>[] m_IpRanges;

        private Tuple<long, long> GenerateIpRange(string range)
        {
            var ipArray = range.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (ipArray.Length != 2)
                throw new ArgumentException("Invalid ipRange exist in configuration!");

            return new Tuple<long, long>(ConvertIpToLong(ipArray[0]), ConvertIpToLong(ipArray[1]));
        }

        private long ConvertIpToLong(string ip)
        {
            var points = ip.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (points.Length != 4)
                throw new ArgumentException("Invalid ipRange exist in configuration!");

            long value = 0;
            long unit = 1;

            for (int i = points.Length - 1; i >= 0; i--)
            {
                value += unit * points[i].ToInt32();
                unit *= 256;
            }

            return value;
        }



    }

}





