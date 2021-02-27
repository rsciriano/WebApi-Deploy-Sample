using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure
{
    public interface IConnectionProvider
    {
        IDbConnection CreateConnection();

        string GetDatabaseType();

    }
}
