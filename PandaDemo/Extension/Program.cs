using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "admin1801235";
            Console.WriteLine(str.ToMD5());
        }
    }
}
