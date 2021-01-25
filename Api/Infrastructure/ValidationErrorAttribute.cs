using System.Linq;
using System.Net;
using System.Net.Http;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Api.Infrastructure
{
    public class ValidationErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            if (exception == null)
            {
                return;
            }

            var errors = exception.Errors
                .Select(x => new { x.PropertyName, x.ErrorMessage })
                .ToArray();

            context.Result = new ObjectResult(errors)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

        }
    }
}