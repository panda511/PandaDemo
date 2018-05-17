using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;

namespace SuperSocketServer1
{
    public class Add : CommandBase<MyAppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(MyAppSession session, StringRequestInfo requestInfo)
        {
            session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
        }
    }

    public class Mult : CommandBase<AppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            var result = 1;

            foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
            {
                result *= factor;
            }

            session.Send(result.ToString());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var appServer = new AppServer();

            var serverConfig = new ServerConfig
            {
                Ip = "192.168.1.73",
                Port = 2012,
                Name = "DyWebSocketServer8",
                //Listeners = list,


                MaxConnectionNumber = 311 //最大连接数，默认100
            };

            //Setup the appServer
            if (!appServer.Setup(serverConfig, connectionFilters: null)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            //新的连接
            appServer.NewSessionConnected += new SessionHandler<AppSession>(AppServer_NewSessionConnected);

            //新的请求
            //appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(AppServer_NewRequestReceived);

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
            session.Send("Welcome to SuperSocket Telnet Server");
            Console.WriteLine("发现一个新的客户端连接"+ session.ToString());
        }

        static void AppServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            switch (requestInfo.Key.ToUpper())
            {
                case ("ECHO"):
                    session.Send(requestInfo.Body);
                    break;

                case ("ADD"):
                    int sum = 0;
                    requestInfo.Parameters.ToList().ForEach(c => sum += Convert.ToInt32(c));
                    session.Send(sum.ToString());
                    break;

                case ("MULT"):
                    var result = 1;

                    foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
                    {
                        result *= factor;
                    }

                    session.Send(result.ToString());
                    break;
            }
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
