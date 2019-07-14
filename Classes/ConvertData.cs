using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;

using duproprioscrapingproject.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using DataScrapingApp.Classes;

namespace DataScrapingApp
{
    public class ConvertDataService : IDisposable
    {

        public bool HtmlToListOfHouses(string url)
        {
            try
            {
                var LastPage = false;
                int i = 1;
                int noofrep = 15;
                bool ToIncrementRep = true;
                int currentrep = 0;
                var category = url.Split('/').ToList().Last().Split('.').First();
                var lastToRemove = url.Split('/').Skip(3).ToList().Last();
                var parentCategoreis = string.Join(" > ", url.Split('/').Skip(3).ToList().Where(x => x != lastToRemove).ToList());
                while (!LastPage)
                {
                    url = url.Split('?')[0] + "?p=" + i;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("visiting " + url);
                    Console.BackgroundColor = ConsoleColor.Black;
                    HtmlWeb web = new HtmlWeb();
                    web.AutoDetectEncoding = false;
                    web.OverrideEncoding = Encoding.Default;
                    HtmlDocument Doc = new HtmlDocument();
                    //Doc.LoadHtml(UsingSelenium(url));
                    Doc = web.Load(url);
                    var lis = Doc.DocumentNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("item")).ToList();
                    //LastPage = Doc.DocumentNode.Descendants("span").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("none-left")).Any();
                    LastPage = true;
                    foreach (var item in lis)
                    {
                        if (item.Descendants("img").FirstOrDefault() != null && item.Descendants("img").First().Attributes["src"].Value != null)
                        {
                            var obj = new Data()
                            {
                                ImageLink = item.Descendants("img").First().Attributes["src"].Value,
                                ProductName = item.Descendants("a").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("list-product-name")).First().InnerText,
                                Link = item.Descendants("a").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("list-product-name")).First().Attributes["href"].Value,
                                Price = item.Descendants("meta").Where(x => x.Attributes.Contains("content")).First().Attributes["content"].Value
                            };
                            obj.BrandName = obj.ProductName.Split(' ')[0].ToUpper().Trim();
                            obj.Category = category;
                            obj.PrentCategories = parentCategoreis;
                            using (var db = new BloggingContext())
                            {

                                if (!db.data.Where(x => x.ProductName == obj.ProductName && x.Price == obj.Price).Any())
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("saving " + obj.ProductName);
                                    Console.ForegroundColor = ConsoleColor.White;
                                    db.data.Add(obj);
                                    db.SaveChanges();
                                    ToIncrementRep = false;
                                    LastPage = false;

                                }
                            }
                        }
                    }
                    if (ToIncrementRep)
                    {
                        currentrep = currentrep + 1;
                    }
                    ToIncrementRep = true;

                    if (currentrep <= noofrep)
                    {
                        LastPage = false;
                    }
                    i++;
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("URL COMPLETED " + url);
                Console.ForegroundColor = ConsoleColor.White;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string UsingSelenium(string url)
        {



            var options = new ChromeOptions()
            {
                BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                AcceptInsecureCertificates = true

            };
            options.AddArguments("headless");
            options.AddArgument("disable-extensions");

            using (IWebDriver driver = new ChromeDriver(@"C:\Users\it\Desktop\DuSearchConsoleApp\FBScrapping\FBScrappingApp", options))
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                driver.Navigate().GoToUrl(url);
                return driver.PageSource;
            }


        }
        public void Dispose() { }
        public void GetMainLinks()
        {
            HtmlWeb web = new HtmlWeb();
            web.AutoDetectEncoding = false;
            web.OverrideEncoding = Encoding.Default;
            HtmlDocument Doc = new HtmlDocument();
            Doc = web.Load("https://www.xcite.com.sa");
            var list = Doc.DocumentNode.Descendants("li").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains("level2 li-cat-ul")).ToList();
            //var list2 = Doc.DocumentNode.Descendants("li").Where(x => x.Attributes.Contains("class")&& x.Attributes["class"].Value.Contains("level1 li-cat-ul")).ToList();

            foreach (var item in list)
            {
                DataHolder.MainLinks.Add(item.Descendants("a").First().Attributes["href"].Value);

            }
            //foreach (var item in list2)
            //{
            //    DataHolder.MainLinks.Add(item.Descendants("a").First().Attributes["href"].Value);

            //}

        }
        public void GetArabicData()
        {
            using (var db = new BloggingContext())
            {
                while (db.data.Where(x => x.IsVisited != true).Any())
                {
                    try
                    {

                        var listOfItems = db.data.Where(x => x.IsVisited != true).Take(100).ToList();
                        foreach (var item in listOfItems)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            var link = item.Link;
                            link = link.Replace("sa/", "sa/ar/");
                            Console.WriteLine("visiting " + link);
                            Console.ForegroundColor = ConsoleColor.White;
                            HtmlWeb web = new HtmlWeb();
                            web.AutoDetectEncoding = true;
                            // web.OverrideEncoding = Encoding.UTF8;
                            HtmlDocument Doc = new HtmlDocument();
                            Doc = web.Load(link);
                            var obj = new ArabicData();
                            obj.Link = link;
                            obj.Category = item.Category;
                            obj.ImageLink = item.ImageLink;
                            obj.PrentCategories = item.PrentCategories;
                            obj.Price = item.Price;
                            if (!Doc.DocumentNode.Descendants("div")
                            .Where(x => x.Attributes.Contains("id") && x.Attributes["id"].Value == "div-404")
                            .Any())
                            {
                                obj.ProductName = Doc.DocumentNode.Descendants("h1")
                                .Where(x => x.Attributes.Contains("itemprop") && x.Attributes["itemprop"].Value == "name")
                                .Select(x => x.InnerText).First();
                                //System.Console.WriteLine(obj.ProductName);
                                var overViewNode = Doc.DocumentNode.Descendants("div")
                                .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "key_features_pdp")
                                .FirstOrDefault();
                                if (overViewNode != null)
                                {
                                    // var OverViewList = overViewNode.Descendants("ul").FirstOrDefault().Descendants("li").Select(x => x.InnerText).ToList();

                                    // obj.QuickOverview = string.Join(",", OverViewList);
                                    obj.QuickOverview = overViewNode.InnerText;
                                }
                                else
                                {
                                    obj.QuickOverview = "Not Given";
                                }
                                //System.Console.WriteLine(obj.QuickOverview);
                                var brandnameNode = Doc.DocumentNode.Descendants("span")
                                                     .Where(x => x.Attributes.Contains("itemprop") && x.Attributes["itemprop"].Value == "brand")
                                                     .Select(x => x.InnerText).FirstOrDefault();
                                if (brandnameNode != null)
                                {
                                    obj.BrandName = brandnameNode.Replace("&nbsp;", " ");

                                }
                                else
                                {
                                    obj.BrandName = obj.ProductName.Split(' ')[0];
                                }
                                //System.Console.WriteLine(obj.BrandName);
                                var descNode = Doc.DocumentNode.Descendants("div")
                                .Where(x => x.Attributes.Contains("id") && x.Attributes["id"].Value == "p-details")
                                .FirstOrDefault()
                                .Descendants("div")
                                .Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "RichContent container")
                                .FirstOrDefault();
                                if (descNode != null)
                                {
                                    obj.Description = descNode.InnerText;
                                }
                                else
                                {
                                    obj.Description = "No Description";
                                }
                            }
                            else
                            {
                                obj.ProductName = "Removed";
                            }
                            //System.Console.WriteLine(obj.Description);
                            item.IsVisited = true;
                            db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.arabicData.Add(obj);
                            db.SaveChanges();

                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        System.Console.WriteLine(db.arabicData.Count() + " items saved!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("EXCEPTION OCCURED -" + ex.Message);
                    }
                }

            }

        }
    }
}