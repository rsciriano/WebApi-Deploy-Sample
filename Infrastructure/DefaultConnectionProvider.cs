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
        private readonly DatabaseContext databaseContext;
        private readonly IDatabaseInitializer<DatabaseContext> databaseInitializer;

        public DefaultConnectionProvider(DatabaseConfiguration<DatabaseContext> configuration, DatabaseContext databaseContext, IDatabaseInitializer<DatabaseContext> databaseInitializer)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            this.databaseInitializer = databaseInitializer ?? throw new ArgumentNullException(nameof(databaseInitializer));
        }

        public DatabaseConfiguration<DatabaseContext> Configuration { get; }

        public IDbConnection CreateConnection()
        {
            string databaseType = Configuration.GetDatabaseType();
            string connectionString = Configuration.GetConnectionString();

            databaseInitializer.EnsureDatabaseInitialization(databaseContext);

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