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
    [Route("api/v{version:apiVersion}/cinemas/{cinemaId:int}/screens")]
    [AuthorizeAttribute(Policy = Policies.Administrator)]
    public class ScreensController : Controller
    {
        private readonly IMediator _mediator;

        public ScreensController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/cinemas/1/screens

        [HttpGet]
        [Route("")]
        public IActionResult GetScreens()
        {
            // TODO: Impement
            return NoContent();
        }

        // GET: api/v1/cinemas/1/screens/1

        [HttpGet]
        [Route("{screenId:int}", Name = "GetScreen")]
        public IActionResult GetScreen(int cinemaId, int screenId)
        {
            // TODO: Implement
            return Ok($"Screen {screenId}");
        }

        // POST: api/v1/cinemas/1/screens

        [HttpPost]
        [Route("")]
        [ValidateModel]
        public async Task<IActionResult> CreateScreen(
            int cinemaId,
            CreateScreenBindingModel model)
        {
            var response = await _mediator.Send(new CreateScreenCommand(
                cinemaId: cinemaId,
                screenName: model.Name,
                screenRows: model.Rows,
                screenSeatsPerRow: model.SeatsPerRow));

            var url = Url.RouteUrl("GetScreen", new
            {
                CinemaId = cinemaId,
                ScreenId = response.Screen.Id
            });

            return Created(url, response.Screen);
        }
    }
}
