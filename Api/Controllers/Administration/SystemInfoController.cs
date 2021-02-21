using Api.Infrastructure.Authorization;
using Api.Infrastructure.Versioning;
using Aplication.Queries;
using Aplication.Queries.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers.Administration
{
    [ApiController]
    [Version1]
    [Version2]
    [Route("api/v{version:apiVersion}/system-info")]
    [AuthorizeAttribute(Policy = Policies.Administrator)]
    public class SystemInfoController: Controller
    {
        private readonly IMediator _mediator;

        public SystemInfoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SystemInfoViewModel))]
        public async Task<IActionResult> GetSystemInfo()
        {
            var response = await _mediator.Send(new GetSystemInfoQuery());

            return Ok(response);

        }

    }
}
