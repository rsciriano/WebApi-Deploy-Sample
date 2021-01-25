using System.Threading.Tasks;
using Api.Infrastructure.Authorization;
using Api.Infrastructure.Versioning;
using Aplication.Queries;
using Aplication.Queries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Administration
{
    [ApiController]
    [Version1]
    [Route("api/v{version:apiVersion}/cinemas")]
    [AuthorizeAttribute(Policy = Policies.Administrator)]
    public class CinemasController : Controller
    {
        private readonly IMediator _mediator;

        public CinemasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/cinemas

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CinemaViewModel[]))]
        public async Task<IActionResult> GetCinemas()
        {
            var response = await _mediator.Send(new GetCinemasQuery());

            return Ok(response.Data);
        }

        // GET: api/v1/cinemas/1
        [HttpGet]
        [Route("{cinemaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CinemaViewModel))]
        public async Task<IActionResult> GetCinema(int cinemaId)
        {
            var response = await _mediator.Send(new GetCinemaQuery
            {
                CinemaId = cinemaId
            });

            return Ok(response.Data);
        }
    }
}
