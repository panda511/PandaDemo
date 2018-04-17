using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringSimilarity
{
    class Program
    {
        public static int GetSimilarity(string source, string target)
        {
            if (source.Length == 0)
            {
                return target.Length;
            }
            if (target.Length == 0)
            {
                return source.Length;
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

                    // find the minimum from the three vars above
                    int cellValue = valueAbove < valueLeft ? (valueDiag < valueAbove ? valueDiag : valueAbove) : (valueDiag < valueLeft ? valueDiag : valueLeft);
                    martix[i, j] = cellValue;
                }
            }

            int result = martix[source.Length, target.Length];

            return result;
        }

        static void Main(string[] args)
        {
            #region name
            string str = @"
                安捷快递              ,
                安能物流              ,
                安信达快递            ,
                北青小红帽            ,
                百福东方              ,
                CCES快递              ,
                城市100               ,
                COE东方快递           ,
                长沙创一              ,
                成都善途速运          ,
                德邦                  ,
                D速物流               ,
                大田物流              ,
                EMS                   ,
                快捷速递              ,
                FEDEX联邦(国内件）    ,
                FEDEX联邦(国际件）    ,
                飞康达                ,
                广东邮政              ,
                共速达                ,
                国通快递              ,
                高铁速递              ,
                汇丰物流              ,
                天天快递              ,
                恒路物流              ,
                天地华宇              ,
                华强物流              ,
                百世快递              ,
                华夏龙物流            ,
                好来运快递            ,
                京广速递              ,
                九曳供应链            ,
                佳吉快运              ,
                嘉里物流              ,
                捷特快递              ,
                急先达                ,
                晋越快递              ,
                加运美                ,
                佳怡物流              ,
                跨越物流              ,
                龙邦快递              ,
                联昊通速递            ,
                民航快递              ,
                明亮物流              ,
                能达速递              ,
                平安达腾飞快递        ,
                全晨快递              ,
                全峰快递              ,
                全日通快递            ,
                如风达                ,
                赛澳递                ,
                圣安物流              ,
                盛邦物流              ,
                上大物流              ,
                顺丰快递              ,
                盛丰物流              ,
                盛辉物流              ,
                速通物流              ,
                申通快递              ,
                速腾快递              ,
                速尔快递              ,
                唐山申通              ,
                全一快递              ,
                优速快递              ,
                万家物流              ,
                万象物流              ,
                新邦物流              ,
                信丰快递              ,
                希优特                ,
                新杰物流              ,
                源安达快递            ,
                远成物流              ,
                韵达快递              ,
                义达国际物流          ,
                越丰物流              ,
                原飞航物流            ,
                亚风快递              ,
                运通快递              ,
                圆通速递              ,
                亿翔快递              ,
                邮政平邮/小包         ,
                增益快递              ,
                汇强快递              ,
                宅急送                ,
                众通快递              ,
                中铁快运              ,
                中通速递              ,
                中铁物流              ,
                中邮物流              ,
                亚马逊物流            ,
                速必达物流            ,
                瑞丰速递              ,
                快客快递              ,
                城际快递              ,
                CNPEX中邮快递         ,
                鸿桥供应链            ,
                海派通物流公司        ,
                澳邮专线              ,
                泛捷快递              ,
                PCA Express           ,
                UEQ Express           
            ";
            #endregion

            string[] similarityList = str.Split(','); 
            string input = "百世";
            string target = input.Replace("快递", "").Trim().ToUpper();

            Dictionary<string, double> dict = new Dictionary<string, double>();

            foreach (string s in similarityList)
            {
                string source = s.Trim().ToUpper();

                double value = GetSimilarity(source, target);
                if (source.Contains(target))
                {
                    value /= 5;
                }

                if (!dict.ContainsKey(source))
                {
                    dict.Add(source, value);
                }

            }

            var dictResut = dict.OrderBy(d => d.Value).Take(10);

            foreach (var item in dictResut)
            {
                Console.WriteLine(item.Key + " " + item.Value);
            }
            Console.Read();
        }
    }
}
