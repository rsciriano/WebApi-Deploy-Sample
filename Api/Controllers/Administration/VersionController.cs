﻿using Api.Infrastructure.Authorization;
using Api.Infrastructure.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers.Administration
{
    [ApiController]
    [Version1]
    [Version2]
    [Route("api/v{version:apiVersion}/version")]
    [AuthorizeAttribute(Policy = Policies.Administrator)]
    public class VersionController : Controller
    {
        [Route("")]
        [HttpGet]
        public string Get() => $"v1.0 on {Environment.OSVersion.ToString()}";

        [Route(""), MapToApiVersion("2.0")]
        [HttpGet]
        public string GetV2() => $"v2.0 on {Environment.OSVersion.ToString()}";
    }
}
