using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Api.Infrastructure
{
    public class ValidateDateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var routeData = actionContext.RouteData;
            var year = int.Parse(routeData.Values["year"].ToString());
            var month = int.Parse(routeData.Values["month"].ToString());
            var day = int.Parse(routeData.Values["day"].ToString());

            if (DateBuilder.TryBuildFrom(year, month, day, out DateTime date) == false)
            {
                actionContext.Result = new ObjectResult(new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Invalid date"

                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}