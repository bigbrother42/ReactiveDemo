﻿using BaseDemo.Util;
using DataDemo.WebDto;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using BaseDemo.Util;
using SharedDemo.Util;
using System;
using System.IO;
using BaseDemo.Util.Extensions;

namespace SharedDemo.GlobalData
{
    public class SqLiteDbContext : DbContext
    {
        private SqliteConnection dbConnection;

        public DbSet<UserBasicInfoWebDto> UserBasicInfo { get; set; }

        public DbSet<NoteCategoryWebDto> NoteCategory { get; set; }

        public DbSet<NoteContentWebDto> NoteContent { get; set; }

        public DbSet<NoteTypeWebDto> NoteType { get; set; }

        public DbSet<SystemConfigWebDto> SystemConfig { get; set; }

        public SqLiteDbContext()
        {

        }

        public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : base(options)
        {

        }

        public SqLiteDbContext(SqliteConnection sqliteConnection)
        {
            dbConnection = sqliteConnection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (optionsBuilder.IsConfigured) return;

            if (!Directory.Exists(SqliteExtensions.DbPath)) Directory.CreateDirectory(SqliteExtensions.DbPath);

            if (dbConnection == null || dbConnection.DataSource.IsNullOrEmpty())
                dbConnection = InitializeSqliteConnection($@"{SqliteExtensions.DbPath}\{SqliteExtensions.SQLiteName}");

            //optionsBuilder.UseLoggerFactory(SqliteExtensions.SqliteLoggerFactory);
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.UseSqlite(dbConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBasicInfoWebDto>().HasKey(o => new { o.UserId });
            modelBuilder.Entity<UserBasicInfoWebDto>(e =>
            {
                // Disable identity (auto-incrementing) on integer primary key
                //e.Property(o => o.UserSeq).ValueGeneratedNever();
                e.Property(o => o.UserId).IsRequired();
                e.Property(o => o.UserName).IsRequired();
                e.Property(o => o.Password).IsRequired();
            });

            modelBuilder.Entity<NoteCategoryWebDto>().HasKey(o => new { o.UserId, o.TypeId, o.CategoryId });
            modelBuilder.Entity<NoteCategoryWebDto>(e =>
            {
                e.Property(o => o.UserId).IsRequired();
                e.Property(o => o.TypeId).IsRequired();
                e.Property(o => o.DisplayOrder).IsRequired();
                e.Property(o => o.CategoryId).IsRequired();
                e.Property(o => o.CategoryName).IsRequired();
            });

            modelBuilder.Entity<NoteContentWebDto>().HasKey(o => new { o.UserId, o.TypeId, o.CategoryId, o.ContentId });
            modelBuilder.Entity<NoteContentWebDto>(e =>
            {
                e.Property(o => o.UserId).IsRequired();
                e.Property(o => o.TypeId).IsRequired();
                e.Property(o => o.CategoryId).IsRequired();
                e.Property(o => o.ContentId).IsRequired().ValueGeneratedOnAdd();
                e.Property(o => o.CategoryId).IsRequired();
            });

            modelBuilder.Entity<NoteTypeWebDto>().HasKey(o => new { o.UserId, o.TypeId });
            modelBuilder.Entity<NoteTypeWebDto>(e =>
            {
                e.Property(o => o.UserId).IsRequired();
                e.Property(o => o.TypeId).IsRequired();
                e.Property(o => o.TypeName).IsRequired();
                e.Property(o => o.DisplayOrder).IsRequired();
                e.Property(o => o.Description).IsRequired();
            });

            modelBuilder.Entity<SystemConfigWebDto>().HasKey(o => new { o.Id});
            modelBuilder.Entity<SystemConfigWebDto>(e =>
            {
                e.Property(o => o.Id).IsRequired();
                e.Property(o => o.UserId).IsRequired();
                e.Property(o => o.FunctionNo).IsRequired();
                e.Property(o => o.ItemNo).IsRequired();
                e.Property(o => o.ItemName).IsRequired();
                e.Property(o => o.ConfigContent).IsRequired();
            });

            // modelBuilder.SetDefaultValueSql(GetType().GetAllSetProperties(), "CURRENT_TIMESTAMP", "CreatedAt", "UpdatedAt");
        }

        private static SqliteConnection InitializeSqliteConnection(string dbFilePath)
        {
            try
            {
                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = dbFilePath,
                    Password = SqliteExtensions.GetSqlitePassword(SqliteExtensions.DbPath)
                };

                return new SqliteConnection(connectionString.ToString());
            }
            catch (Exception e)
            {
                LogUtil.Instance.Error(e);

                throw new Exception(e.Message);
            }
        }
    }
}
