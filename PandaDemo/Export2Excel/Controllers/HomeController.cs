using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WSSS.Helpers;

namespace Export2Excel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Export()
        {
            var list = GetData();
            string excelName = "abcd";
            //string excelName = "人员信息";
            MemoryStream memoryStream = ExcelUtil.ExportExcel(list, excelName);
            using (memoryStream)
            {
                return File(memoryStream.ToArray(), "application/vnd.ms-excel", excelName + ".xls");
            }
        }

        public List<Person> GetData()
        {
            List<Person> list = new List<Person>();

            Person a = new Person()
            {
                Name = "张三",
                Age = 23,
                Birthday = new DateTime(1974, 6, 21)
            };

            Person b = new Person()
            {
                Name = "李四",
                Age = 36,
                Birthday = DateTime.Now
            };

            list.Add(a);
            list.Add(b);

            return list;
        }
    }
    public class Person
    {
        [Description("姓名")]
        public string Name { get; set; }

        [Description("年龄")]
        public int Age { get; set; }

        [Description("出生日期")]
        public DateTime Birthday { get; set; }

        [Description("出生")]
        public string BirthTime { get { return Birthday.ToString("yyyy-MM-dd HH:mm:ss"); } }
    }
   
}