using System;
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
            using (StreamReader sr = new StreamReader(@"E:\ff.txt"))
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        private static void Write(string message)
        {
            string filePath = "shipping_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Append))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
                fileStream.Close();
            }


        }
    }
}
