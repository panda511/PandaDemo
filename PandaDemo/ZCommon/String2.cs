using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCommon.Extention;

namespace ZCommon
{
    public class String2
    {
        public static string GetGuid(string format = "N")
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 计算两个字符串的相似度
        /// </summary>
        public static int CalcSimilarity(string source, string target)
        {
            if (source.IsNullOrEmpty() || target.IsNullOrEmpty())
            {
                return 0;
            }

            int[,] martix = new int[source.Length + 1, target.Length + 1];
            for (int i = 0; i <= source.Length; i++)
            {
                martix[i, 0] = i;
            }
            for (int j = 0; j <= target.Length; j++)
            {
                martix[0, j] = j;
            }

            for (int i = 1; i <= source.Length; i++)
            {
                char tempSource = source[i - 1];
                int cost = 0;
                for (int j = 1; j <= target.Length; j++)
                {
                    char tempTarget = target[j - 1];
                    if (tempTarget == tempSource)
                    {
                        cost = 0;
                    }
                    else
                    {
                        cost = 1;
                    }

                    int valueAbove = martix[i - 1, j] + 1;
                    int valueLeft = martix[i, j - 1] + 1;
                    int valueDiag = martix[i - 1, j - 1] + cost;

                    int cellValue = valueAbove < valueLeft ? (valueDiag < valueAbove ? valueDiag : valueAbove) : (valueDiag < valueLeft ? valueDiag : valueLeft);
                    martix[i, j] = cellValue;
                }
            }

            int result = martix[source.Length, target.Length];

            return result;
        }
    }
}
