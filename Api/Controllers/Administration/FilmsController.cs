using Api.Infrastructure;
using Api.Infrastructure.Authorization;
using Api.Infrastructure.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Administration
{
    [ApiController]
    [Version1]
    [Route("api/v{version:apiVersion}/films")]
    [AuthorizeAttribute(Policy = Policies.Administrator)]
    public class FilmsController : Controller
    {
        private readonly IMediator _mediator;

        public FilmsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/films

        [HttpGet]
        [Route("")]
        public IActionResult GetFilms()
        {
            return NoContent(); 
        }

        // GET: api/v1/films/1

        [HttpGet]
        [Route("{filmId:int}")]
        public IActionResult GetFilm(int filmId)
        {
            return NoContent();
        }

        // POST: api/v1/films

        [HttpPost]
        [Route("")]
        [ValidateModel]
        public IActionResult CreateFilm(
            string model)
        {
            return NoContent();
        }
    }
}
