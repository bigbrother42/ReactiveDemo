using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SharedDemo.GlobalData;

namespace ReactiveDemo.Filters
{
    public class CreateSQLiteDbConnectionFilter : AbstractFilter
    {
        public override object Handle(object request)
        {
            CreateDbConnection();

            return base.Handle(request);
        }

        private void CreateDbConnection()
        {
            using (var dbContext = new SqLiteDbContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.OpenConnection();
                GlobalData.DbConnection = (SqliteConnection)dbContext.Database.GetDbConnection();
            }
        }
    }
}
