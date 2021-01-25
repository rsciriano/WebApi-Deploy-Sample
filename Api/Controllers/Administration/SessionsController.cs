using System;
using System.Threading.Tasks;
using Api.BindingModels;
using Api.Infrastructure;
using Api.Infrastructure.Authorization;
using Api.Infrastructure.Versioning;
using Aplication.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Administration
{
    [ApiController]
    [Version1]
    [Route("api/v{version:apiVersion}/cinemas/{cinemaId:int}/sessions")]
    [AuthorizeAttribute(Policy = Policies.Administrator)]
    public class SessionsController : Controller
    {
        private readonly IMediator _mediator;

        public SessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/cinemas/1/sessions

        [HttpGet]
        [Route("")]
        public IActionResult GetSessions()
        {
            // TODO: Implement
            return NoContent();
        }

        // GET: api/v1/cinemas/1/sessions/1

        [HttpGet]
        [Route("{sessionId:int}", Name = "GetSession")]
        public IActionResult GetSession(int cinemaId, int sessionId)
        {
            // TODO: Implement
            return Ok($"Session {sessionId}");
        }

        // POST: api/v1/cinemas/1/sessions

        [HttpPost]
        [Route("")]
        [ValidateModel]
        public async Task<IActionResult> CreateSession(
            int cinemaId,
            CreateSessionBindingModel model)
        {
            var response = await _mediator.Send(new CreateSessionCommand(
                cinemaId: cinemaId,
                screenId: model.ScreenId,
                filmId: model.FilmId,
                start: model.Start));

            var url = Url.RouteUrl("GetSession", new { CinemaId = cinemaId, SessionId = response.Session.SessionId });
            return Created(url, response.Session);
        }

        // DELETE: cinemas/1/sessions/1
        [HttpDelete]
        [Route("{sessionId:int}")]
        public Task<IActionResult> DeleteSession(
            int cinemaId,
            int sessionId)
        {
            // TODO: Implement
            throw new NotImplementedException();
        }

        // PUT: api/v1/cinemas/1/sessions/1/publish

        [HttpPut]
        [Route("{sessionId:int}/publish")]
        [ValidateModel]
        public async Task<IActionResult> PublishSession(int cinemaId, int sessionId)
        {
            await _mediator.Send(new PublishSessionCommand(
                cinemaId: cinemaId,
                sessionId: sessionId,
                action: PublishSessionCommand.ActionType.Publish));

            return NoContent();
        }

        // DELETE: cinemas/1/sessions/1/publish

        [HttpDelete]
        [Route("{sessionId:int}/publish")]
        [ValidateModel]
        public async Task<IActionResult> UnpublishSession(int cinemaId, int sessionId)
        {
            await _mediator.Send(new PublishSessionCommand(
                cinemaId: cinemaId,
                sessionId: sessionId,
                action: PublishSessionCommand.ActionType.Publish));

            return NoContent();
        }

        // List sessions (Pubished and unpublished)
        // Create sessions
        // Update session
        // Delete session

        // Publish sessions for day
        // Unpublish sessions for day
        // Publish session
        // Unpublish session
    }
}
