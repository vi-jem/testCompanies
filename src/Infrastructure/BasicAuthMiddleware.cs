using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace myrestful.Infrastructure
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string path = context.Request?.Path;
            if(path.Contains("search", StringComparison.InvariantCultureIgnoreCase))
            {
                await _next.Invoke(context);
            }
            else
            {
                string authHeader = context.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic"))
                {
                    string encodedUsernamePassword = authHeader.Split(' ')[1];
                    string[] usernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword)).Split(':');
                    string username = usernamePassword[0];
                    string password = usernamePassword[1];

                    if (username == "aladdin" && password == "opensesame")
                    {
                        await _next.Invoke(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                }
                else
                // no authorization header
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
        }
    }
}