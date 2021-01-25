using Microsoft.AspNetCore.Mvc;
using System;


namespace Api.Infrastructure.Versioning
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class Version1Attribute : ApiVersionAttribute
    {
        public Version1Attribute() : base(new ApiVersion(1, 0))
        {
            Deprecated = false;
        }
    }
}
