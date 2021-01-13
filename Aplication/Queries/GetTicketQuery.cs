using Aplication.Queries.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Queries
{
    public class GetTicketQuery: IRequest<QueryResponse<TicketViewModel>>
    {
        public int CinemaId { get; set; }

        public Guid TicketId { get; set; }

    }
}
