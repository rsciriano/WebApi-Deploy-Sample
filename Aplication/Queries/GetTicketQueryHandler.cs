using Aplication.Queries.Infrastructure;
using Aplication.Queries.ViewModels;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Queries
{
    public class GetTicketQueryHandler : IAsyncRequestHandler<GetTicketQuery, QueryResponse<TicketViewModel>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetTicketQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task<QueryResponse<TicketViewModel>> Handle(GetTicketQuery message)
        {
            //TODO: Restrict response depend on user's permissions

            using (var conn = _connectionProvider.CreateConnection())
            {
                const string sql = @"
SELECT 
	T.Id
	, SS.SessionId
	, SC.CinemaId
	, T.SessionSeat_SeatRow
	, T.SessionSeat_SeatNumber
	, T.Price

FROM [ticket].[Tickets] as T
INNER JOIN [session].[SessionSeats] as SS ON SS.TicketId = T.Id
INNER JOIN [session].[Sessions] as S ON S.Id = SS.SessionId
INNER JOIN [cine].[Screens] as SC ON SC.Id = S.ScreenId

WHERE SC.CinemaId = @cinemaId AND T.id = @TicketId
";
                var tickets = await conn.QueryAsync<TicketViewModel>(sql, new
                {
                    CinemaId = message.CinemaId,
                    TicketId = message.TicketId
                });

                if (tickets.Any())
                {

                    return new QueryResponse<TicketViewModel>
                    {
                        Data = tickets.Single()
                    };
                }
                else
                {
                    return null;
                }
            }

        }
    }
}
