using System;
using System.Threading.Tasks;
using Api.Infrastructure;
using Api.Infrastructure.Authorization;
using Api.Infrastructure.Versioning;
using Aplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Scheduling
{
    [ApiController]
    [Version1]
    [Route("api/v{version:apiVersion}/cinemas/{cinemaId:int}/schedule")]
    [AuthorizeAttribute(Policy = Policies.Administrator)]
    public class ScheduleController : Controller
    {
        private readonly IMediator _mediator;

        public ScheduleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/cinemas/1/schedule/2017/4/3

        [HttpGet]
        [Route("{year:int}/{month:range(1,12)}/{day:range(1,31)}")]
        //[ValidateDate] // Date validation as filter
        public async Task<IActionResult> GetSchedule(
            int cinemaId,
            int year,
            int month,
            int day)
        {
            if (DateBuilder.TryBuildFrom(year, month, day, out DateTime date) == false)
            {
                return BadRequest();
            }

            var response = await _mediator.Send(new GetScheduleQuery
            {
                CinemaId = cinemaId,
                Date = date
            });

            return Ok(response.Data);
        }
    }
}
