using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace WSSS.Helpers
{
    public class ExcelUtil
    {
        #region 从List导出Excel

        public static MemoryStream ExportExcel<T>(List<T> list, string excelHeader)
        {
            HSSFWorkbook hssfworkbook = ExportListToHSSFWorkbook(list, excelHeader);

            MemoryStream memoryStream = new MemoryStream();
            hssfworkbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static byte[] GetExcelByte<T>(List<T> list, string excelHeader)
        {
            byte[] result = null;

            HSSFWorkbook hssfworkbook = ExportListToHSSFWorkbook(list, excelHeader);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                hssfworkbook.Write(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                result = memoryStream.ToArray();
            }

            return result;
        }

        public static void SetCellValue(Type modePropertyType, ICell newCell, string drValue)
        {
            if (string.IsNullOrEmpty(drValue))
            {
                return;
            }
            if (modePropertyType == typeof(string))
            {
                newCell.SetCellValue(drValue);
                return;
            }
            else if (modePropertyType == typeof(DateTime))
            {
               // newCell.SetCellValue(Convert.ToDateTime(drValue));
                newCell.SetCellValue(drValue);
                return;
            }
            else if (modePropertyType == typeof(DateTime?))
            {
                //newCell.SetCellValue(Convert.ToDateTime(drValue));
                newCell.SetCellValue(drValue);
                return;
            }
            else if (modePropertyType == typeof(float?))
            {
                newCell.SetCellValue(Convert.ToSingle(drValue));
                return;
            }
            else if (modePropertyType == typeof(decimal?))
            {
                newCell.SetCellValue(Convert.ToDouble(drValue));
                return;
            }
            else if (modePropertyType == typeof(double?))
            {
                newCell.SetCellValue(Convert.ToDouble(drValue));
                return;
            }
            else if (modePropertyType == typeof(int?))
            {
                newCell.SetCellValue(Convert.ToInt32(drValue));
                return;
            }
            else if (modePropertyType == typeof(int))
            {
                newCell.SetCellValue(Convert.ToInt32(drValue));
                return;
            }

            else if (modePropertyType == typeof(float))
            {
                newCell.SetCellValue(Convert.ToSingle(drValue));
                return;
            }

            else if (modePropertyType == typeof(double))
            {
                newCell.SetCellValue(Convert.ToDouble(drValue));
                return;
            }

            else if (modePropertyType == typeof(decimal))
            {
                newCell.SetCellValue(Convert.ToDouble(drValue));
                return;
            }

            else if (modePropertyType == typeof(sbyte))
            {
                newCell.SetCellValue(Convert.ToSByte(drValue));
                return;
            }
            else if (modePropertyType == typeof(sbyte?))
            {
                newCell.SetCellValue(Convert.ToSByte(drValue));
                return;
            }
            else if (modePropertyType == typeof(byte))
            {
                newCell.SetCellValue(Convert.ToByte(drValue));
                return;
            }
            else if (modePropertyType == typeof(byte?))
            {
                newCell.SetCellValue(Convert.ToByte(drValue));
                return;
            }
            else if (modePropertyType == typeof(short))
            {
                newCell.SetCellValue(Convert.ToInt16(drValue));
                return;
            }
            else if (modePropertyType == typeof(short?))
            {
                newCell.SetCellValue(Convert.ToInt16(drValue));
                return;
            }
            else if (modePropertyType == typeof(ushort))
            {
                newCell.SetCellValue(Convert.ToUInt16(drValue));
                return;
            }
            else if (modePropertyType == typeof(ushort?))
            {
                newCell.SetCellValue(Convert.ToUInt16(drValue));
                return;
            }

            else if (modePropertyType == typeof(uint))
            {
                newCell.SetCellValue(Convert.ToUInt32(drValue));
                return;
            }
            else if (modePropertyType == typeof(uint?))
            {
                newCell.SetCellValue(Convert.ToUInt32(drValue));
                return;
            }
            else if (modePropertyType == typeof(long))
            {
                newCell.SetCellValue(Convert.ToInt64(drValue));
                return;
            }
            else if (modePropertyType == typeof(long?))
            {
                newCell.SetCellValue(Convert.ToInt64(drValue));
                return;
            }
            else if (modePropertyType == typeof(ulong))
            {
                newCell.SetCellValue(Convert.ToUInt64(drValue));
                return;
            }
            else if (modePropertyType == typeof(ulong?))
            {
                newCell.SetCellValue(Convert.ToUInt64(drValue));
                return;
            }

            else if (modePropertyType == typeof(bool))
            {
                newCell.SetCellValue(Convert.ToBoolean(drValue));
                return;
            }
            else if (modePropertyType == typeof(bool?))
            {
                newCell.SetCellValue(Convert.ToBoolean(drValue));
                return;
            }

            else if (modePropertyType == typeof(char))
            {
                newCell.SetCellValue(Convert.ToChar(drValue));
                return;
            }
            else if (modePropertyType == typeof(char?))
            {
                newCell.SetCellValue(Convert.ToChar(drValue));
                return;
            }
            else
            {
                newCell.SetCellValue(drValue);
            }
        }

        private static HSSFWorkbook ExportListToHSSFWorkbook<T>(List<T> list, string excelHeader)
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
            IRow rowFirst = sheet.CreateRow(1);
            int rowIndex = 2;
            int colIndex = 0;
            foreach (T t in list)
            {
                IRow row = sheet.CreateRow(rowIndex++);
                colIndex = 0;
                foreach (PropertyInfo pi in properties)
                {
                    string desc = GetPropertyDescription<T>(pi.Name);
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
                        //val = "2017-10-12 12:12:12";
                        cell.SetCellValue(val); 
                        //SetCellValue(pi.PropertyType, cell, value == null ? "" : value.ToString().Trim());//根据字段类型设置单元格类型
                        cell.CellStyle = style;
                        //sheet.AutoSizeColumn(colIndex - 1, true);//这里去掉 否则单元格多的情况下会特别慢
                    }
                }
            }

            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, colIndex - 1));
            sheet.GetRow(0).Height = 500;

            return hssfworkbook;
        }

        private static string GetClassDescription<T>()
        {
            System.ComponentModel.AttributeCollection attributes = TypeDescriptor.GetAttributes(typeof(T));
            DescriptionAttribute da = (DescriptionAttribute)attributes[typeof(DescriptionAttribute)];
            return da.Description;
        }

        private static string GetPropertyDescription<T>(string propertyName)
        {
            PropertyDescriptor pd = TypeDescriptor.GetProperties(typeof(T))[propertyName];
            DescriptionAttribute da = pd.Attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
            return da.Description;
        }

        #endregion
    }
}