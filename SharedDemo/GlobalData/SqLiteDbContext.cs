using BaseDemo.Util;
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

            optionsBuilder.UseLoggerFactory(SqliteExtensions.SqliteLoggerFactory);
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.UseSqlite(dbConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBasicInfoWebDto>().HasKey(o => o.UserId);
            modelBuilder.Entity<UserBasicInfoWebDto>(e =>
            {
                // Disable identity (auto-incrementing) on integer primary key
                //e.Property(o => o.UserSeq).ValueGeneratedNever();
                e.Property(o => o.UserId).IsRequired();
                e.Property(o => o.UserName).IsRequired();
                e.Property(o => o.Password).IsRequired();
            });

            modelBuilder.SetDefaultValueSql(GetType().GetAllSetProperties(), "CURRENT_TIMESTAMP", "CreatedAt", "UpdatedAt");
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
