using System;
using System.Linq;
using Domain.Aggregates.Cinemas;
using Domain.Aggregates.Films;
using Domain.Aggregates.Sessions;
using FluentAssertions;
using Xunit;

namespace Domain.UnitTests.Aggregates.Sessions
{
    public class SessionSeatTests
    {
        private readonly Screen _screen;
        private readonly Film _film;

        private readonly Session _session;
        private readonly SessionSeat _sut;


        public SessionSeatTests()
        {
            _screen = new Screen(new Cinema("Cinemundo"), "Guara", 2, 2);
            _film = new Film("Pulp fiction", 120);

            _session = Session.Create(_screen, _film, DateTime.Today.AddHours(17));

            _sut = _session.Seats.First();
        }

        [Fact]
        public void Can_Sell_Unsold_Seat()
        {
            // Act
            _sut.Sell(5);

            // Assert
            _sut.Sold.Should().BeTrue();
        }

        [Fact]
        public void Cannot_Sell_Sold_Seat()
        {
            // Arrange
            _sut.Sell(5);

            // Act
            var action = new Action(() => _sut.Sell(3));

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Tiket_Creaste_When_Sell_Unsold_Seat()
        {
            // Act
            _sut.Sell(5.12m);

            // Assert
            _sut.Ticket.Should().NotBeNull();
            _sut.Ticket.Should().NotBeNull();
            _sut.Ticket.Id.Should().Be(_sut.TicketId.Value);
            _sut.Ticket.Price.Should().Be(5.12m);
            _sut.Ticket.SessionSeat.Should().BeEquivalentTo(_sut);
        }


    }
}
