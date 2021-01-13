using Api.IntegrationTests.Infrastructure;
using Api.IntegrationTests.Infrastructure.CollectionFixtures;
using Aplication.Queries;
using Aplication.Queries.ViewModels;
using FluentAssertions;
using Microsoft.Owin.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Api.IntegrationTests.Specs.Ticketing
{
    [Collection(Collections.Database)]
    public class GetTicket
    {
        private readonly DatabaseFixture _fixture;

        private readonly int _cinemaId;
        private readonly int _publishedSessionId;
        private readonly Guid _ticketId;

        public GetTicket(DatabaseFixture fixture)
        {
            _fixture = fixture;

            _cinemaId = _fixture.SeedData.Cinema.Id;
            _publishedSessionId = _fixture.SeedData.Sessions.First().Id;
            _ticketId = _fixture.SeedData.Sessions.First().Seats.First(s => s.TicketId.HasValue).TicketId.Value;
        }

        [Fact]
        public async Task GetTicket_By_Id_For_Cinema_Should_Return_TicketInfo()
        {
            var endpoint = $"api/v1/cinemas/{_cinemaId}/ticketing/{_ticketId}";
            var response = await _fixture.Server.CreateRequest(endpoint)
                .WithIdentity(Identities.User)
                .GetAsync();

            await response.IsSuccessStatusCodeOrTrow();

            var value = await response.Content.ReadAsAsync<QueryResponse<TicketViewModel>>();

            value.Should().NotBeNull();
            value.Data.Should().NotBeNull();
            value.Data.Id.Should().Be(_ticketId);
        }

    }
}
