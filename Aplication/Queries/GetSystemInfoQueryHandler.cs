using System.Threading.Tasks;
using Aplication.Queries.ViewModels;
using MediatR;
using Dapper;
using Infrastructure;
using System;

namespace Aplication.Queries
{
    public class GetSystemInfoQueryHandler : IAsyncRequestHandler<GetSystemInfoQuery, SystemInfoViewModel>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetSystemInfoQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<SystemInfoViewModel> Handle(GetSystemInfoQuery message)
        {
            var systemInfo = new SystemInfoViewModel();

            systemInfo.MachineName = Environment.MachineName;
            systemInfo.OSVersion = Environment.OSVersion.ToString();
            systemInfo.DatabaseType = _connectionProvider.GetDatabaseType();

            using (var conn = _connectionProvider.CreateConnection())
            {
                string sql = null;
                if (systemInfo.DatabaseType == "mssql")
                {
                    sql = "select @@version";
                }
                else if (systemInfo.DatabaseType == "postgres")
                {
                    sql = "select version()";
                }

                if (sql != null)
                {
                    try
                    {
                        systemInfo.DatabaseVersionString = await conn.ExecuteScalarAsync<string>(sql);
                    }
                    catch(Exception ex)
                    {
                        // For demo purposes only ;-)
                        systemInfo.DatabaseVersionString = ex.ToString();
                    }
                }
                else
                {
                    systemInfo.DatabaseVersionString = "¿?";
                }

            }

            return systemInfo;
        }
    }
}
