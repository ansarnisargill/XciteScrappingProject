using duproprioscrapingproject.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DataScrapingApp.Classes
{
    public static class DataHolder
    {
        public static int NoOfList { get; set; } = 1;
        public static List<string> MainLinks { get; set; } = new List<string>();
        public static bool ListToExcel<T>(List<T> query)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Result");

                //get our column headings
                var t = typeof(T);
                var Headings = t.GetProperties();
                for (int i = 0; i < Headings.Count(); i++)
                {

                    ws.Cells[1, i + 1].Value = Headings[i].Name;
                }

                //populate our Data
                if (query.Count() > 0)
                {
                    ws.Cells["A2"].LoadFromCollection(query);
                }

                //Format the header
                using (ExcelRange rng = ws.Cells["A1:BZ1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                using (FileStream fs = new FileStream(@"C:\Users\Student\Desktop\XciteScrappingProject\xciteArabicData" + NoOfList + ".xlsx", FileMode.Create))
                {
                    pck.SaveAs(fs);
                }
                NoOfList += 1;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(NoOfList + ".xlsx was added!");
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }

        }
        public static void Page_Load()
        {
            var datatable = GetDataTableFromExcel(@"C:\Users\Student\Desktop\XciteScrappingProject\xciteData1.xlsx");
            int i = 0;

            foreach (DataRow row in datatable.Rows)
            {
                using (var db = new BloggingContext())
                {
                    var obj = new Data();
                    obj.BrandName = row[nameof(obj.BrandName)].ToString();
                    obj.Category = row[nameof(obj.Category)].ToString();
                    obj.ImageLink = row[nameof(obj.ImageLink)].ToString();
                    obj.Link = row[nameof(obj.Link)].ToString();
                    obj.PrentCategories = row[nameof(obj.PrentCategories)].ToString();
                    obj.Price = row[nameof(obj.Price)].ToString();
                    obj.ProductName = row[nameof(obj.ProductName)].ToString();
                    db.data.Add(obj);
                    db.SaveChanges();
                    System.Console.WriteLine(obj.ProductName + " saved!");
                    i++;
                }

            }
            System.Console.WriteLine("Saving complete " + i);
        }

        private static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
    }
}
