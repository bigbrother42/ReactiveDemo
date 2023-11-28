using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BaseDemo.Util;
using System.Reflection;

namespace SharedDemo.Util
{
    public static  class SqliteExtensions
    {
        // The secret key is used both for encryption and decryption
        private static readonly byte[] AES256_KEY = Encoding.UTF8.GetBytes(@"7x!A%D*G-JaNdRgUkXp2s5v8y/B?E(H+");

        // The initialization vertor used for CBC mode
        private static readonly byte[] AES256_IV = Encoding.UTF8.GetBytes(@"fTjWnZr4u7x!A%D*");

        public static readonly ILoggerFactory SqliteLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        /// <summary>
        /// SQLite path
        /// </summary>
        public static readonly string DbPath = $@"{System.Windows.Forms.Application.StartupPath}\..\data\db\karte";

        /// <summary>
        /// SQLite Name
        /// </summary>
        public static readonly string SQLiteName = "karte.sqlite3";

        /// <summary>
        /// Password file name
        /// </summary>
        public static readonly string Mpp = "karte.mpp";

        /// <summary>
        /// Get sqlite password from local file
        /// </summary>
        /// <returns></returns>
        public static string GetSqlitePassword(string dbPath)
        {
            if (!Directory.Exists(dbPath))
                return null;

            var passwordPath = $@"{dbPath}\{Mpp}";

            var text = GetFileText(passwordPath);

            return text.IsNullOrWhiteSpace()
                ? null
                : EncryptionUtility.DecryptStringByAes(EncryptionUtility.DecryptStringByAes(text, AES256_KEY, AES256_IV), AES256_KEY, AES256_IV);
        }

        public static string GetFileText(string path)
        {
            if (!File.Exists(path))
                return null;
            try
            {
                return File.ReadAllText(path, Encoding.UTF8);
            }
            catch (Exception e)
            {
                LogUtil.Instance.Error($"Read sqlite password error. {e}");
                return null;
            }
        }

        #region Sqlite

        /// <summary>
        /// Configure the default value expression for the column of all DbSet properties of a DbContext
        /// </summary>
        /// <param name="modelBuilder">Provide a simple API surface for configuring relationships between our model and database</param>
        /// <param name="properties">The dbset properties</param>
        /// <param name="sql">The SQL expression for default value of the column</param>
        /// <param name="propertyNames">The name of the property to be configured</param>
        public static void SetDefaultValueSql(this ModelBuilder modelBuilder, IEnumerable<PropertyInfo> properties, string sql, params string[] propertyNames)
        {
            foreach (var propertyInfo in properties)
            {
                var type = propertyInfo.PropertyType.GetGenericArguments()[0];
                foreach (var propertyName in propertyNames)
                {
                    if (type.GetProperty(propertyName) != null)
                        modelBuilder.Entity(type).Property(propertyName).HasDefaultValueSql(sql);
                }
            }
        }

        /// <summary>
        /// Removes all rows in a table, execute direct SQL instead of remove the enitities one by one from the set
        /// will be more efficient.
        /// </summary>
        /// <param name="dbContext">The database context</param>
        /// <param name="type">The DbSet's type</param>
        /// <returns></returns>
        public static void ClearTable(this DbContext dbContext, Type type)
        {
            var tableName = type.Name.Substring(0, type.Name.Length - 6);
            dbContext.Database.ExecuteSqlRaw($"DELETE FROM {tableName}");
        }

        /// <summary>
        /// Rebuilds the entire database to reclaim empty spaces, or "free" database pages.
        /// </summary>
        /// <param name="dbContext"></param>
        public static void Vacuum(this DbContext dbContext)
        {
            dbContext.Database.ExecuteSqlRaw("VACUUM");
        }

        /// <summary>
        /// 指定されたテーブルの構造を表示する
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static List<string> PragmaTableInfo(this DatabaseFacade database, string tableName)
        {
            var fileid = new List<string>();
            try
            {
                if (tableName.IsNullOrWhiteSpace()) return null;
                string querySql = $"PRAGMA table_info({tableName});";
                using (var command = database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = querySql;
                    command.CommandType = CommandType.Text;
                    database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            fileid.Add(result["name"] as string);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.Instance.Error($"Error:{tableName}:{e}");
            }
            return fileid;
        }

        /// <summary>
        /// Synchronized executes a raw sql in EF Core with specific mapper to convert raw results to a <see cref="T"/> list  
        /// </summary>
        /// <typeparam name="T">The results type</typeparam>
        /// <param name="database">The database</param>
        /// <param name="querySql">The raw query sql</param>
        /// <param name="mapper">The mapper to convert raw results</param>
        /// <returns></returns>
        [Obsolete]
        public static List<T> ExecuteRawSqlQuery<T>(this DatabaseFacade database, string querySql, Func<DbDataReader, T> mapper)
        {
            if (querySql.IsNullOrWhiteSpace())
                return null;
            using (var command = database.GetDbConnection().CreateCommand())
            {
                command.CommandText = querySql;
                command.CommandType = CommandType.Text;
                database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(mapper(result));
                    }

                    return entities;
                }
            }
        }

        /// <summary>
        /// Synchronized executes a raw sql in EF Core and auto convert raw results to a <see cref="T"/> list
        /// </summary>
        /// <typeparam name="T">The results type</typeparam>
        /// <param name="database">The database</param>
        /// <param name="querySql">The raw query sql</param>
        /// <param name="param">The parameters of the Transact-SQL statement </param>
        /// <returns></returns>
        public static List<T> ExecuteRawSqlQueryAutoMapper<T>(this DatabaseFacade database, string querySql, Action<DbCommand> param = null) where T : class, new()
        {
            if (querySql.IsNullOrWhiteSpace())
                return null;
            using (var command = database.GetDbConnection().CreateCommand())
            {
                command.CommandText = querySql;
                command.CommandType = CommandType.Text;
                param?.Invoke(command);
                if (database.GetDbConnection().State != ConnectionState.Open)
                    database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            var obj = new T();
                            MapDataToObject(result, obj);
                            entities.Add(obj);
                        }
                    }

                    return entities;
                }
            }
        }

        /// <summary>
        /// Maps a SqlDataReader record to an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader"></param>
        /// <param name="obj"></param>
        public static void MapDataToObject<T>(this DbDataReader dataReader, T obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            // Fast Member Usage
            var props = obj.GetType().GetProperties();

            var propDic = props.ToDictionary(o => o.Name, o => o);

            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                var columName = dataReader.GetName(i);
                var value = dataReader.IsDBNull(i) ? null : dataReader.GetValue(i);

                if (propDic.ContainsKey(columName))
                {
                    if (value == null)
                    {
                        if (!propDic[columName].PropertyType.IsValueType && Nullable.GetUnderlyingType(propDic[columName].PropertyType) != null)
                            propDic[columName].SetValue(obj, null);
                    }
                    else
                        propDic[columName].SetValue(obj, Convert.ChangeType(value, propDic[columName].PropertyType));
                }
            }
        }

        /// <summary>
        /// Convert Sqlite field to boolean
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool ToBoolean(string field)
        {
            return field == "1";
        }

        #endregion
    }
}
