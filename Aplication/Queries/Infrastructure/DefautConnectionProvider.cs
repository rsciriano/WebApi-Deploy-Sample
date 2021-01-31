using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Aplication.Queries.Infrastructure
{
    public class DefautConnectionProvider : IConnectionProvider
    {
        public DefautConnectionProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IDbConnection CreateConnection()
        {
            var connectionString = Configuration.GetConnectionString("cinematic");
            return new SqlConnection(connectionString);
        }
    }
}