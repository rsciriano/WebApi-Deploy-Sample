using System.Threading.Tasks;
using Aplication.Queries.Infrastructure;
using Aplication.Queries.ViewModels;
using MediatR;
using Dapper;
using System.Threading;

namespace Aplication.Queries
{
    public class GetCinemaQueryHandler : IRequestHandler<GetCinemaQuery, QueryResponse<CinemaViewModel>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetCinemaQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<QueryResponse<CinemaViewModel>> Handle(GetCinemaQuery message, CancellationToken cancellationToken)
        {
            using (var conn = _connectionProvider.CreateConnection())
            {
                const string sql = @"
SELECT C.Id, C.Name
FROM cine.Cinemas C
WHERE C.Id = @id
";
                var cinema = await conn.QueryFirstOrDefaultAsync<CinemaViewModel>(sql, new { Id = message.CinemaId });

                return new QueryResponse<CinemaViewModel>
                {
                    Data = cinema
                };
            }
        }
    }
}
