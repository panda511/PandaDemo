using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extension
{
    public static class ListExtention
    {
        #region AvoidNull

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

        #endregion

        #region ToExcel

        public static MemoryStream ToExcel<T>(this List<T> list, string excelHeader = null)
        {
            MemoryStream memoryStream = new MemoryStream();

            string header = excelHeader ?? GetClassExcel<T>();
            HSSFWorkbook hssfworkbook = list.ToWorkbook(header);
            hssfworkbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        private static HSSFWorkbook ToWorkbook<T>(this List<T> list, string excelHeader)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet("Sheet1");

            if (list == null || list.Count == 0)
            {
                return hssfworkbook;
            }

            //表头
            ICell cellHeader = sheet.CreateRow(0).CreateCell(0);
            cellHeader.SetCellValue(excelHeader);
            ICellStyle styleHeader = hssfworkbook.CreateCellStyle();
            styleHeader.Alignment = HorizontalAlignment.Center;
            styleHeader.VerticalAlignment = VerticalAlignment.Center;
            IFont fontHeader = hssfworkbook.CreateFont();
            fontHeader.FontHeightInPoints = 14;
            styleHeader.SetFont(fontHeader);
            cellHeader.CellStyle = styleHeader;

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

            ICellStyle style = hssfworkbook.CreateCellStyle();
            IFont font = hssfworkbook.CreateFont();
            font.FontName = "宋体";
            font.FontHeightInPoints = 11;
            style.SetFont(font);
            style.DataFormat = HSSFDataFormat.GetBuiltinFormat("text"); //单元格格式-文本
            IRow rowFirst = sheet.CreateRow(1);
            int rowIndex = 2;
            int colIndex = 0;
            foreach (T t in list)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                colIndex = 0;
                foreach (PropertyInfo pi in properties)
                {
                    string desc = GetPropertyExcel<T>(pi.Name);
                    if (!string.IsNullOrEmpty(desc))
                    {
                        object value = pi.GetValue(t, null);
                        ICell cell = null;
                        if (rowIndex == 3)
                        {
                            cell = rowFirst.CreateCell(colIndex);
                            cell.SetCellValue(desc);
                            cell.CellStyle = style;
                        }
                        cell = row.CreateCell(colIndex++);
                        string val = "";
                        if (value != null)
                        {
                            if (pi.PropertyType == typeof(DateTime))
                            {
                                val = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                val = value.ToString().Trim();
                            }
                        }
                        cell.SetCellValue(val);
                        cell.CellStyle = style;
                        //sheet.AutoSizeColumn(colIndex - 1, true);//单元格多的情况下会特别慢
                    }
                }
            }

            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, colIndex - 1));
            sheet.GetRow(0).Height = 500;

            return hssfworkbook;
        }

        private static string GetPropertyExcel<T>(string propertyName)
        {
            string title = string.Empty;

            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                var attribute = propertyInfo.GetCustomAttributes(typeof(ExcelAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    title = ((ExcelAttribute)attribute).Title;
                }
            }

            return title;
        }

        private static string GetClassExcel<T>()
        {
            string title = string.Empty;

            var attribute = typeof(T).GetCustomAttributes(typeof(ExcelAttribute), false).FirstOrDefault();

            if (attribute != null)
            {
                title = ((ExcelAttribute)attribute).Title;
            }

            return title;
        }

        #endregion
    }

    public class ExcelAttribute : Attribute
    {
        public string Title { get; }

        public ExcelAttribute(string title)
        {
            Title = title;
        }
    }
}
