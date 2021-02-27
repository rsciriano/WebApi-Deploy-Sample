using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DatabaseConfiguration<TContext> where TContext: DbContext
    {
        public DatabaseConfiguration(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private IConfiguration Configuration { get; }

        public string GetDatabaseType()
        {
            return Configuration["DatabaseType"];
        }

        public string GetConnectionString()
        {
            string connectionStringName = $"Cinematic_{GetDatabaseType()}";
            string connectionString = Configuration.GetConnectionString(connectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Conection string '{connectionStringName}' not configured");
            }

            return connectionString;
        }

        public DbContextOptions<TContext> GetDbContextOptions() 
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            string databaseType = GetDatabaseType();
            string connectionString = GetConnectionString();

            if (databaseType == "mssql")
            {
                optionsBuilder.UseSqlServer(connectionString);

            }
            else if (databaseType == "postgres")
            {
                optionsBuilder.UseNpgsql(connectionString);
            }
            else
            {
                throw new InvalidOperationException($"'{databaseType}' is a invalid value for DatabaseType configuration. Valid values are 'mssql', 'postgres'");
            }

            return optionsBuilder.Options;
        }

    }
}
