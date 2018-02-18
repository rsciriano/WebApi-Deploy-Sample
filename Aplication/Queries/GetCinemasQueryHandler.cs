using System.Linq;
using System.Threading.Tasks;
using Aplication.Queries.Infrastructure;
using Aplication.Queries.ViewModels;
using MediatR;
using Dapper;
using System.Threading;

namespace Aplication.Queries
{
    public class GetCinemasQueryHandler : IRequestHandler<GetCinemasQuery, QueryResponse<CinemaViewModel[]>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetCinemasQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<QueryResponse<CinemaViewModel[]>> Handle(GetCinemasQuery message, CancellationToken cancellationToken)
        {
            using (var conn = _connectionProvider.CreateConnection())
            {
                // TODO: Add paging, sorting and filtering
                const string sql = @"
SELECT TOP 10 C.Id, C.Name 
FROM cine.Cinemas C
";
                var cinemas = await conn.QueryAsync<CinemaViewModel>(sql);

                return new QueryResponse<CinemaViewModel[]>
                {
                    Data = cinemas.ToArray()
                };
            }
        }
    }
}
