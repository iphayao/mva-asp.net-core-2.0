using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareExample
{
    public class EnvironmentMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly IHostingEnvironment _Env;

        public EnvironmentMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _Next = next;
            _Env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            var timer = Stopwatch.StartNew();
            context.Response.Headers.Add("X-HostingEnvironment", new[] { _Env.EnvironmentName });

            await _Next(context);

            if(_Env.EnvironmentName == "Development" && 
                context.Response.ContentType != null && 
                context.Response.ContentType == "text/html")
            {
                await context.Response.WriteAsync($"<p>From {_Env.EnvironmentName} in {timer.ElapsedMilliseconds}ms</p>");
            }
        }
    }

    public static class MiddlewareHelpers
    {
        public static IApplicationBuilder UseEnvironmentMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<EnvironmentMiddleware>();
        }
    }
}
