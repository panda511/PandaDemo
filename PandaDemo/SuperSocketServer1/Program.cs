using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketServer1
{
    //public class Add : CommandBase<MyAppSession, StringRequestInfo>
    //{
    //    public override void ExecuteCommand(MyAppSession session, StringRequestInfo requestInfo)
    //    {
    //        session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
    //    }
    //}

    //public class Mult : CommandBase<AppSession, StringRequestInfo>
    //{
    //    public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
    //    {
    //        var result = 1;

    //        foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
    //        {
    //            result *= factor;
    //        }

    //        session.Send(result.ToString());
    //    }
    //}


    class Program
    {
        static int count = 0;
        static int count2 = 0;

        static void Main(string[] args)
        {
            var appServer = new AppServer();

            var serverConfig = new ServerConfig
            {
                Ip = "192.168.1.73",
                Port = 2012,
                Name = "DyWebSocketServer8",
                //Listeners = list,
                MaxConnectionNumber = 50000 //最大连接数，默认100
            };

            if (!appServer.Setup(serverConfig, connectionFilters: null))
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            //新的连接
            appServer.NewSessionConnected += new SessionHandler<AppSession>(AppServer_NewSessionConnected);

            //断开连接
            appServer.SessionClosed += new SessionHandler<AppSession, CloseReason>(AppServer_SessionClosed);

            //新的请求
            appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(AppServer_NewRequestReceived);


            Timer timer = new Timer((data) =>
            {
                //对当前已连接的符合条件的会话进行广播
                var sessions = appServer.GetAllSessions();// appServer.GetSessions(c => c.MyCustomerId == "1");
                if (sessions != null)
                {
                    foreach (var session in sessions)
                    {
                        session.Send("广播消息测试" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }

            }, null, 10000, 10000);




            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        static void AppServer_NewSessionConnected(AppSession session)
        {
            session.Send("Welcome to SuperSocket Server");
            Console.WriteLine("发现一个新的客户端连接" + (count++) + ": " + session.RemoteEndPoint);
        }

        static void AppServer_SessionClosed(AppSession session, CloseReason reason)
        {
            Console.WriteLine("一个客户端断开了连接" + (count2++) + ": " + session.RemoteEndPoint);
        }

        static void AppServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine(requestInfo.ToString());
            session.Send(requestInfo.Body);

            //switch (requestInfo.Key.ToUpper())
            //{
            //    case ("ECHO"):
            //        session.Send(requestInfo.Body);
            //        break;
            //    case ("ADD"):
            //        int sum = 0;
            //        requestInfo.Parameters.ToList().ForEach(c => sum += Convert.ToInt32(c));
            //        session.Send(sum.ToString());
            //        break;
            //    case ("MULT"):
            //        var result = 1;
            //        foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
            //        {
            //            result *= factor;
            //        }
            //        session.Send(result.ToString());
            //        break;
            //}
        }

    }

    public class MyAppSession : AppSession<MyAppSession>
    {
        public int MyId { get; internal set; }
        public string MyName { get; internal set; }

        protected override void OnSessionStarted()
        {
            this.Send("Welcome to SuperSocket Telnet Server");
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            this.Send("Unknow request");
        }

        protected override void HandleException(Exception e)
        {
            this.Send("Application error: {0}", e.Message);
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            //add you logics which will be executed after the session is closed
            base.OnSessionClosed(reason);
        }
    }

    public class MyAppServer : AppServer<MyAppSession>
    {

    }
}
