using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    public static class ListExtention
    {
        public static void ForEach2<T>(this List<T> list, Action<T> action)
        {
            if (list != null && list.Count > 0)
            {
                list.ForEach(action);
            }
        }

        public static T FindLast2<T>(this List<T> list, Predicate<T> match)
        {
            if (list != null && list.Count > 0)
            {
                return list.FindLast(match);
            }
            else
            {
                return default(T);
            }
        }

        public static T Find2<T>(this List<T> list, Predicate<T> match)
        {
            if (list != null && list.Count > 0)
            {
                return list.Find(match);
            }
            else
            {
                return default(T);
            }
        }

        public static List<T> FindAll2<T>(this List<T> list, Predicate<T> match)
        {
            if (list != null && list.Count > 0)
            {
                return list.FindAll(match);
            }
            else
            {
                return null;
            }
        }
    }
}
