using Domain.Aggregates.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregates.Tickets
{
    public class Ticket
    {
        public Guid Id { get; init; }
        public SessionSeat SessionSeat { get; init; }
        public decimal Price { get; init; }
    }
}
