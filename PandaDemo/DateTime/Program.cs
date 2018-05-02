using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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



        #region 线程

        static void Main2(string[] args)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(DoSth));
            thread.Start(2);
            Console.WriteLine("main");
            Console.Read();
        }

        static void DoSth(object i)
        {
            Console.WriteLine("doSth" + i);
        }

        static void Main22(string[] args)
        {
            BookShop book = new BookShop();

            //创建两个线程同时访问Sale方法
            Thread t1 = new Thread(new ThreadStart(book.Sale));
            Thread t2 = new Thread(new ThreadStart(book.Sale));

            //启动线程
            t1.Start();
            t2.Start();

            Console.ReadKey();

        }

        class BookShop
        {
            //剩余图书数量
            public int num = 1;
            public void Sale()
            {
                //使用lock关键字解决线程同步问题
                lock (this)
                {
                    int tmp = num;
                    if (tmp > 0)//判断是否有书，如果有就可以卖
                    {
                        Thread.Sleep(1000);
                        num -= 1;
                        Console.WriteLine("售出一本图书，还剩余{0}本", num);
                    }
                    else
                    {
                        Console.WriteLine("没有了");
                    }
                }
            }
        }

        #endregion




    }


    #region AbstractFactory

    public abstract class YaBo
    {
        public abstract void Print();
    }

    public abstract class YaJia
    {
        public abstract void Print();
    }

    public class NCYaBo : YaBo
    {
        public override void Print()
        {
            Console.WriteLine("南昌鸭脖");
        }
    }

    public class SHYaBo : YaBo
    {
        public override void Print()
        {
            Console.WriteLine("上海鸭脖");
        }
    }

    public class NCYaJia : YaJia
    {
        public override void Print()
        {
            Console.WriteLine("南昌鸭架");
        }
    }

    public class SHYaJia : YaJia
    {
        public override void Print()
        {
            Console.WriteLine("上海鸭架");
        }
    }

    public abstract class AbstractFactory
    {
        public abstract YaBo CreateYaBo();
        public abstract YaJia CreateYaJia();
    }

    public class NCFactory : AbstractFactory
    {
        public override YaBo CreateYaBo()
        {
            return new NCYaBo();
        }

        public override YaJia CreateYaJia()
        {
            return new NCYaJia();
        }
    }

    public class SHFactory : AbstractFactory
    {
        public override YaBo CreateYaBo()
        {
            return new SHYaBo();
        }

        public override YaJia CreateYaJia()
        {
            return new SHYaJia();
        }
    }

    #endregion

}
