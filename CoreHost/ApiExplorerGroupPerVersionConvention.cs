using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreHost
{
    public class ApiExplorerGroupPerVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var apiVersions = controller.Attributes.Where(a => a is ApiVersionAttribute).Cast<ApiVersionAttribute>();

            foreach (var apiVersion in apiVersions)
            {
                foreach (var item in apiVersion.Versions)
                {
                    controller.ApiExplorer.GroupName = $"v{item.MajorVersion}.{item.MinorVersion}";
                }
            }
        }
    }
}
