using DataScrapingApp.Classes;
using duproprioscrapingproject.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DataScrapingApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("App has started!");
            //SaveData();
            // using (var db = new BloggingContext())
            // {
            //     saveFile(db.data.ToList());
            // }
            SaveData();
        }
        static void SaveData()
        {


            using (ConvertDataService obj = new ConvertDataService())
            {
                //obj.GetMainLinks();
                //var list = DataHolder.MainLinks.SkipWhile(x=>x!=@"https://www.xcite.com.sa/phones/phone-accessories/cases-sleeves-backpacks.html").ToList();


                //foreach (var item in list)
                //{
                //obj.HtmlToListOfHouses(@"https://www.xcite.com.sa/phones/phone-accessories/cases-sleeves-backpacks.html");
                // using(var db=new BloggingContext())
                // {
                //     Console.ForegroundColor = ConsoleColor.Cyan;
                //     Console.WriteLine("Total saved"+db.data.Count());
                //     Console.ForegroundColor = ConsoleColor.White;
                // }
                //}
                obj.GetArabicData();

            }


        }
        static void saveFile(List<Data> list)
        {
            DataHolder.ListToExcel(list);
        }
        static void ReadExcelAndSave()
        {
            DataHolder.Page_Load();
        }
    }
}
