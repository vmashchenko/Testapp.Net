using System;
using System.Net;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using TestAppApi.Models;

namespace TestAppApi.Infrastructure.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public const string DefaultErrorMessage = "Internal Server Error";

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Exception error = contextFeature.Error;

                        string message = DefaultErrorMessage;

                        if (error is ArgumentException argumentException)
                        {
                            message = argumentException.Message;
                        }

                        var errorMsg = new ErrorModelInfo()
                        {
                            Message = message,
                            ErrorObject = error.Message,
                            StatusCode = context.Response.StatusCode,
                        };

                        await context.Response.WriteAsync(errorMsg.ToString());
                    }
                });
            });
        }
    }
}
