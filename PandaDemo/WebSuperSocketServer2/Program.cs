﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;

namespace WebSuperSocketServer2
{
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


    class Program
    {
        static void Main(string[] args)
        {
            //从配置文件启动

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
}
