using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IDatabaseInitializer<TContext> where TContext : DbContext
    {
        bool EnsureDatabaseInitialization(TContext context);

        void InitializeDatabase(TContext context);

        void Seed(TContext context);
    }
}
