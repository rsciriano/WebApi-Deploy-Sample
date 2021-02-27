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
        private readonly Lazy<DatabaseContext> lazyDatabaseContext;
        private readonly IDatabaseInitializer<DatabaseContext> databaseInitializer;

        public DefaultConnectionProvider(DatabaseConfiguration<DatabaseContext> configuration, Lazy<DatabaseContext> lazyDatabaseContext, IDatabaseInitializer<DatabaseContext> databaseInitializer)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.lazyDatabaseContext = lazyDatabaseContext ?? throw new ArgumentNullException(nameof(lazyDatabaseContext));
            this.databaseInitializer = databaseInitializer ?? throw new ArgumentNullException(nameof(databaseInitializer));
        }

        public DatabaseConfiguration<DatabaseContext> Configuration { get; }

        public IDbConnection CreateConnection()
        {
            string databaseType = Configuration.GetDatabaseType();
            string connectionString = Configuration.GetConnectionString();

            databaseInitializer.EnsureDatabaseInitialization(lazyDatabaseContext.Value);

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

        public string GetDatabaseType()
        {
            return Configuration.GetDatabaseType();
        }
    }
}