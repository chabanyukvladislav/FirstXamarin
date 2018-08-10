﻿using FirstXamarin.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.IO;
using Xamarin.Forms;

namespace FirstXamarin.DatabaseContext
{
    public sealed class Context : DbContext
    {
        private const string DatabaseName = "database.db";

        public DbSet<PhonesBook> PhonesBooks { get; set; }

        public Context()
        {
            Database.EnsureCreated();
            PhonesBooks.CountAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath;
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
                    break;
                case Device.iOS:
                    Batteries.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", DatabaseName);
                    break;
                case Device.UWP:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DatabaseName);
                    break;
                default:
                    throw new NotImplementedException("Platform not supported");
            }
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }
    }
}