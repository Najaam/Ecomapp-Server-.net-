using eCommrece.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Text.Json;

namespace eCommrece.SharedLibrary.Middleware
{
    public class GlobalExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
// declear defualt VAriables
            string message = "Sorry, Internal Server Error Occurred. Kindly try again later.";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string Title = "Error";     

            try
            {
                await next(context);

                //check if excpetion here is to many request // 429 status code
                if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                {
                    Title = "Warning";
                    message = "Too many requests. Please try again later.";
                    statusCode = StatusCodes.Status429TooManyRequests;
                    await ModifyHeader(context, Title, message, statusCode);
                }
                // check if the Response is unauthorized // 401 status code
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    Title = "Alert";
                    message = "You are not authorized to access this resource.";
                    statusCode = StatusCodes.Status401Unauthorized;
                    await ModifyHeader(context, Title, message, statusCode);
                }
                // check if the Response is forbidden // 403 status code
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    Title = "Out Of Access";
                    message = "You do not have permission to access this resource.";
                    statusCode = StatusCodes.Status403Forbidden;
                    await ModifyHeader(context, Title, message, statusCode);
                }
            }
            catch (Exception ex)
            {
                //Log Orignal Exception /file, debugger, console
                LogException.LogExceptions(ex);

                // check if Exceeption is time out or not // 408 status code
                if(ex is TaskCanceledException || ex is TimeoutException)
                {
                    Title = "Out Of Time";
                    message = "The request has timed out. Please try again later.";
                    statusCode = StatusCodes.Status408RequestTimeout;
                }
                await ModifyHeader(context, Title, message, statusCode);
            }
        }

        private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(new ProblemDetails()
            {
                Detail = message,
                Status = statusCode,
                Title = title,

            }),CancellationToken.None);
            return;
        }
    }
}
