using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Person, PersonOut>()
               .ForMember(pOut => pOut.AgeOut, p => p.MapFrom(c => c.Age))
               .ForMember(pOut => pOut.NameOut, p => p.MapFrom(c => c.Name))
               .ForMember(pOut => pOut.BirthdayOut, p => p.MapFrom(c => c.Birthday.ToString("yyyy-MM-dd HH:mm:ss")))
           );

            Person p1 = new Person() { Age = 12, Name = "张三", Birthday = DateTime.Now };
            Person p2 = new Person() { Age = 63, Name = "李四", Birthday = DateTime.Parse("1996-12-30 15:03:12") };
            List<Person> list = new List<Person>();
            list.Add(p1);
            list.Add(p2);

            //单个映射
            PersonOut personOut = Mapper.Map<PersonOut>(p1);
            Console.WriteLine(personOut.AgeOut + "  " + personOut.NameOut + "  " + personOut.BirthdayOut);

            //List映射
            List<PersonOut> outList = Mapper.Map<List<PersonOut>>(list);
            outList.ForEach(c => Console.WriteLine(c.AgeOut + "  " + c.NameOut + "  " + c.BirthdayOut));

            Console.Read();
        }
    }

    public class Person
    {
        public int Age { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }
    }


    public class PersonOut
    {
        public int AgeOut { get; set; }

        public string NameOut { get; set; }

        public string BirthdayOut { get; set; }

    } 
}
