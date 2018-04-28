using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliPay
{
    class Program
    {
        static void Main2(string[] args)
        {
            AliPay pay = new AliPay();
            decimal amount = 0.03m;
            string orderNo = "123456789";
            string subject = "iphone7 黑色 64G";
            string body = "京东商城"+ subject+ orderNo;
            string str = pay.GetWebPayParameter(amount, orderNo, subject, body);
            Console.WriteLine(str);
            Console.Read();
        }

        static void Main(string[] args)
        {
           
        }
    }

    
}
