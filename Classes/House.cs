using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore;


namespace duproprioscrapingproject.Models
{
    public class BloggingContext : DbContext
    {
        public DbSet<Data> data { get; set; }
        public DbSet<ArabicData> arabicData{get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=C:\Users\Student\Downloads\DuSearchConsoleApp-master\DuSearchConsoleApp-master\XCITE Scrapping App\data.db");
        }
    }
    public class Data
    {
        [Key]
        public int ID { get; set; }

        public string Category { get; set; } = "";
        public string PrentCategories { get; set; } = "";
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string Price { get; set; }
        public string Link { get; set; }
        public string ImageLink { get; set; }
        public string QuickOverview { get; set; }
        public string Description { get; set; }
        public bool? IsVisited { get; set; }


        public Data()
        {
        }
    }
    public class ArabicData
    {
        [Key]
        public int ID { get; set; }

        public string Category { get; set; } = "";
        public string PrentCategories { get; set; } = "";
        public string ProductName { get; set; }
        public string BrandName { get; set; }
        public string Price { get; set; }
        public string Link { get; set; }
        public string ImageLink { get; set; }
        public string QuickOverview { get; set; }
        public string Description { get; set; }


        public ArabicData()
        {
        }
    }
}