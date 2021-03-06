using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.IntegrationTests.Infrastructure;
using Api.IntegrationTests.Infrastructure.CollectionFixtures;
using Aplication.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Api.IntegrationTests.Specs.Ticketing
{
    [Collection(Collections.Database)]
    public class SellSessionSeat
    {
        private readonly DatabaseFixture _fixture;

        private readonly int _cinemaId;
        private readonly int _publishedSessionId;

        public SellSessionSeat(DatabaseFixture fixture)
        {
            _fixture = fixture;

            _cinemaId = _fixture.SeedData.Cinema.Id;
            _publishedSessionId = _fixture.SeedData.Sessions.First().Id;
        }

        [Fact]
        public async Task SellSessionSeat_For_Published_Session_And_Not_Sold_Seat_Should_Return_TicketId()
        {
            var (seatRow, seatNumber) = _fixture.SeedData.Sessions.First().Seats.Where(s => !s.Sold).Select(s => (s.SeatRow, s.SeatNumber)).First();

            var endpoint = $"api/v1/cinemas/{_cinemaId}/ticketing/sessions/{_publishedSessionId}/seat/{seatRow}/{seatNumber}";
            var response = await _fixture.Server.CreateRequest(endpoint)
                .WithIdentity(Identities.User)
                .PostAsync();

            await response.IsSuccessStatusCodeOrTrow();

            var value = await response.Content.ReadAsAsync<SellSessionSeatResponse>();

            value.Should().NotBeNull();
            value.TicketId.Should().NotBeEmpty();
        }
    }
}