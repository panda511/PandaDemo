﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReadWrite
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader(@"E:\ff.txt");
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
}
