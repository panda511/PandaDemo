using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.WebSocket;

namespace WebSuperSocketServer
{
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
                    Port = 4141
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
                    //Ip = "127.0.0.1",
                    //Port = 4141,
                    Listeners = list,
                    //Name = "TestName",
                    //ServerType = "",
                    MaxConnectionNumber = 311 //最大连接数，默认100
                };

                //bool s1 = server.Setup("127.0.0.1", 4141); //对要侦听的IP及端口进行设置
                bool s1 = server.Setup(serverConfig);
                bool s2 = server.Start(); //启动侦听
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
            //Console.WriteLine("有新的连接:" + session.Origin);
            Console.WriteLine("有新的连接:" + (i++));
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
