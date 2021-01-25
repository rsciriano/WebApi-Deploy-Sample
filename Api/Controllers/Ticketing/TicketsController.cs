using System;
using System.Threading.Tasks;
using Api.Infrastructure.Authorization;
using Api.Infrastructure.Versioning;
using Aplication.Commands;
using Aplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Ticketing
{
    [ApiController]
    [Version1]
    [Route("api/v{version:apiVersion}/cinemas/{cinemaId:int}/ticketing")]
    [AuthorizeAttribute(Policy = Policies.Vendor)]
    public class TicketsController : Controller
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: cinemas/1/ticketing/sessions/1/seat/1/2

        [HttpPost]
        [Route("sessions/{sessionId:int}/seat/{row:int}/{number:int}")]
        public async Task<IActionResult> SellSessionSeat(
            int cinemaId,
            int sessionId,
            int row,
            int number)
        {
            // TODO: Add information about the client (student, etc)
            // to be able to call a prices service in the commandHandler

            var response = await _mediator.Send(new SellSessionSeatCommand(
                cinemaId: cinemaId,
                sessionId: sessionId,
                row: row,
                number: number));

            return Ok(response);
        }


        [HttpGet]
        [Route("{ticketId:guid}")]
        public async Task<IActionResult> SellSessionSeat(
            int cinemaId,
            Guid ticketId)
        {
            //TODO: Restrict response depend on user's permissions

            var response = await _mediator.Send(new GetTicketQuery
            {
                CinemaId = cinemaId,
                TicketId = ticketId
            });

            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        // Undo sell ticket for session
        // Available seats per session


    }
}
