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
using SuperSocket.SocketBase.Metadata;
using SuperSocket.SocketEngine;
using SuperSocket.WebSocket;

namespace WebSuperSocketServer
{
    public class LogTimeCommandFilter : CommandFilterAttribute
    {
        public override void OnCommandExecuting(CommandExecutingContext commandContext)
        {
            commandContext.Session.Items["StartTime"] = DateTime.Now;
        }

        public override void OnCommandExecuted(CommandExecutingContext commandContext)
        {
            var session = commandContext.Session;
            var startTime = session.Items.GetValue<DateTime>("StartTime");
            var ts = DateTime.Now.Subtract(startTime);

            if (ts.TotalSeconds > 5 && session.Logger.IsInfoEnabled)
            {
                session.Logger.InfoFormat("A command '{0}' took {1} seconds!", commandContext.CurrentCommand.Name, ts.ToString());
            }
        }
    }








    /// <summary>
    /// 身份认证过滤器
    /// </summary>
    public class AuthorizeFilter : IConnectionFilter
    {
        private bool IsAuthenticated = false;

        public string Name { get; private set; }

        private IAppServer se = null;

        public bool Initialize(string name, IAppServer appServer)
        {
            Name = name;
            se = appServer;

            //Console.WriteLine(appServer.SessionCount + "^^^^^^^^^^^^^^^");

            return true;
        }

        public bool AllowConnect(IPEndPoint remoteAddress)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("**********AuthorizeFilter:" + remoteAddress);
            //Console.WriteLine(remoteAddress);
            //Console.WriteLine("*******************");
            //Console.WriteLine(Name);

            //SuperSocket.SocketBase.i

            var ee = se;

            IsAuthenticated = true;
            return IsAuthenticated;
        }
    }



  


    class Program
    {
        static int i = 1;

        static void Main(string[] args)
        {
            var server = new MyWebSocketServer(); 

            server.NewSessionConnected += ServerNewSessionConnected;
            //server.NewSessionConnected += new SessionHandler<MyWebSocketSession>(ServerNewSessionConnected);
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
                    Ip = "192.168.1.73",
                    Port = 2012,
                    Name = "DyWebSocketServer8",
                    //Listeners = list,
                    MaxConnectionNumber = 311 //最大连接数，默认100
                };


                var filter = new AuthorizeFilter();
                filter.Initialize("DyWebSocketServer8", server);
                //filter.AllowConnect()

                var filters = new List<AuthorizeFilter>() { filter };

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


        public static void ServerNewSessionConnected(MyWebSocketSession session)
        {
            //Console.WriteLine("有新的连接:" + (i++));

            //Console.WriteLine(session.Host);
            //Console.WriteLine(session.Path);
            Console.WriteLine("-------新的连接:" + session.Host + session.Path);

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

        protected override void OnSessionStarted()
        {
            Console.WriteLine("-------SessionStarted:" + this.UriScheme);
            this.CloseWithHandshake(403, "a非法身份c");
            //this.ses
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            //add your business operations
        }
    }

    class MyWebSocketServer : WebSocketServer<MyWebSocketSession>
    {
        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }

    }
}


