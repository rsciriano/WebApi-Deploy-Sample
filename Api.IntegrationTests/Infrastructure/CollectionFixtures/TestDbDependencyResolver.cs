using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;

namespace Api.IntegrationTests.Infrastructure.CollectionFixtures
{
    public class TestDbDependencyResolver : IDbDependencyResolver, IProviderInvariantName, IDbProviderFactoryResolver
    {
        public string Name => "System.Data.SqlClient.SqlClientFactory";

        public object GetService(Type type, object key)
        {
            // Output the service attempting to be resolved along with it's key 
            System.Diagnostics.Debug.WriteLine(string.Format("MyDependencyResolver.GetService({0}, {1})", type.Name, key == null ? "" : key.ToString()));

            if (type == typeof(DbProviderFactory))
            {
                return SqlClientFactory.Instance;
            }
            else if (type == typeof(IProviderInvariantName) || type == typeof(IDbProviderFactoryResolver))
            {
                return this;
            }
            else if (type == typeof(DbProviderServices))
            {
                return SqlProviderServices.Instance;
            }

            return null;
        }

        public IEnumerable<object> GetServices(Type type, object key)
        {
            return new object[] { GetService(type, key) }.ToList().Where(o => o != null);
        }

        public DbProviderFactory ResolveProviderFactory(DbConnection connection)
        {
            return SqlClientFactory.Instance;
        }
    }
}
