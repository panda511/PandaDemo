using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTime2
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt1 = new DateTime(2013, 5, 14);
            DateTime dt2 = new DateTime(2013, 5, 16);
            TimeSpan ts = dt2 - dt1; 
        }
    }
}
