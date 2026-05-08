
using Microsoft.AspNetCore.Http;

namespace eCommrece.SharedLibrary.Middleware
{
    public class ListenToOnlyApiGatway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            //Extract specific header from the request
            var SignedHeader = context.Request.Headers["Api-Gateway"];

            // null means request not coming from api gateway// 503 means service unavailable
            if (SignedHeader.FirstOrDefault() == null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Service Unavailable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
