using Domain.Aggregates.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Queries.ViewModels
{
    public class TicketViewModel
    {
        public Guid Id { get; set; }

        public int SesionId { get; set; }

        public decimal Price { get; set; }

        public int SeatRow { get; set; }

        public int SeatNumber { get; set; }

        public TicketViewModel FromTicket(Ticket ticket)
        {
            if (ticket is null)
            {
                return null;
            }

            return new TicketViewModel
            {
                Id = ticket.Id,
                SesionId = ticket.SessionSeat.SessionId,
                SeatNumber = ticket.SessionSeat.SeatNumber,
                SeatRow = ticket.SessionSeat.SeatRow,
                
                Price = ticket.Price
            };
        }
    }
}
