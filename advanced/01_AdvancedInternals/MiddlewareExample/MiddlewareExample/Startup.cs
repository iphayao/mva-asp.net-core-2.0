using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MiddlewareExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);
            var logger = loggerFactory.CreateLogger("Middleware Demo");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (context, next) => 
            //{
            //    var timer = Stopwatch.StartNew();
            //    logger.LogInformation($"==> beginning request in {env.EnvironmentName}");

            //    await next();

            //    logger.LogInformation($"==> completed requeste {timer.ElapsedMilliseconds}ms");

            //});

            app.UseEnvironmentMiddleware();

            app.Map("/Contacts", a => a.Run(async context => {
                await context.Response.WriteAsync("Here are you contacts:");
            }));

            app.MapWhen(context => context.Request.Headers["User-Agent"].First().Contains("Firefox"), FirefoxRoute);

            app.UseStaticFiles();

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private void FirefoxRoute(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello Firefox");
            });
        }
    }
}
