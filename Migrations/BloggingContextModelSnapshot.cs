﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using duproprioscrapingproject.Models;

namespace DataScrapingApp.Migrations
{
    [DbContext(typeof(BloggingContext))]
    partial class BloggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("duproprioscrapingproject.Models.ArabicData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrandName");

                    b.Property<string>("Category");

                    b.Property<string>("Description");

                    b.Property<string>("ImageLink");

                    b.Property<string>("Link");

                    b.Property<string>("PrentCategories");

                    b.Property<string>("Price");

                    b.Property<string>("ProductName");

                    b.Property<string>("QuickOverview");

                    b.HasKey("ID");

                    b.ToTable("arabicData");
                });

            modelBuilder.Entity("duproprioscrapingproject.Models.Data", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrandName");

                    b.Property<string>("Category");

                    b.Property<string>("Description");

                    b.Property<string>("ImageLink");

                    b.Property<bool?>("IsVisited");

                    b.Property<string>("Link");

                    b.Property<string>("PrentCategories");

                    b.Property<string>("Price");

                    b.Property<string>("ProductName");

                    b.Property<string>("QuickOverview");

                    b.HasKey("ID");

                    b.ToTable("data");
                });
#pragma warning restore 612, 618
        }
    }
}