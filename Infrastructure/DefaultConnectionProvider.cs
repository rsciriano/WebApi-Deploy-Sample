using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DefaultConnectionProvider : IConnectionProvider
    {
        public DefaultConnectionProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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

        public IDbConnection CreateConnection()
        {
            string databaseType = GetDatabaseType();
            string connectionString = GetConnectionString();

            if (databaseType == "mssql")
            {
                return new SqlConnection(connectionString);
            }
            else if (databaseType == "postgres")
            {
                return new NpgsqlConnection(connectionString);
            }
            else
            {
                throw new InvalidOperationException($"'{databaseType}' is a invalid value for DatabaseType configuration. Valid values are 'mssql', 'postgres'");
            }
        }

        public DbContextOptions<T> GetDbContextOptions<T>() where T : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();

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