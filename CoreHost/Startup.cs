using Api;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Infrastructure.Initializers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer();

            /*
            services.AddMvc(c =>
                c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())
            );
            */

            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddApplicationPart(typeof(ApiAutofacModule).Assembly);

            services.AddApiVersioning();

            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiSample", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "WebApiSample", Version = "v2" });
                c.ResolveConflictingActions(resolver => resolver.First());

                /*c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });*/
            });


            ApiConfiguration.Configure(services);
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you by the factory.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new ApiAutofacModule
            {
                SampleConfiguration = true
            });

            builder.RegisterModule<InfrastructureAutofacModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiSample v1.0");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiSample v2.0");
                });

                using (var scope = app.ApplicationServices.CreateScope())
                {
                    using (var context = app.ApplicationServices.GetRequiredService<DatabaseContext>())
                    {
                        //context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        Initiaizer.Seed(context);
                        context.SaveChanges();
                    }
                }
            }

                app.UseRouting();

            // ### Temporal authentication middleware
            if (env.IsDevelopment())
            {
                app.Use(async (context, next) =>
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Role, "Administrator"),
                        new Claim(ClaimTypes.Role, "Vendor"),
                        new Claim(ClaimTypes.Name, "Hugo"),
                    },
                    "Custom");
                    context.User = new ClaimsPrincipal(identity);
                    await next();
                });
                // ###
            }

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
