using System;
using Domain.Aggregates.Cinemas;
using Domain.Aggregates.Tickets;

namespace Domain.Aggregates.Sessions
{
    public class SessionSeat
    {
        protected SessionSeat() { }

        public SessionSeat(Session session, Seat seat)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            if (seat == null)
            {
                throw new ArgumentNullException(nameof(seat));
            }

            Session = session;
            SessionId = Session.Id;
            Seat = seat;
            SeatScreenId = seat.ScreenId;
            SeatRow = seat.Row;
            SeatNumber = seat.Number;
        }

        public int SessionId { get; private set; }

        public Session Session { get; private set; }

        public int SeatScreenId { get; private set; }

        public int SeatRow { get; private set; }

        public int SeatNumber { get; private set; }

        public Seat Seat { get; private set; }

        public decimal? Price { get; private set; }

        public Guid? TicketId { get; set; }

        public Ticket Ticket { get; private set; }

        public bool Sold => TicketId.HasValue;

        public Guid Sell(decimal price)
        {
            if (Sold)
            {
                throw new InvalidOperationException();
            }

            Price = price;

            var ticketId = Guid.NewGuid();
            Ticket = new Ticket
            {
                Id = ticketId,
                Price = price,
                SessionSeat = this
            };

            TicketId = ticketId;

            return ticketId;
        }
    }
}