using Dominate.Data.Model;
using Dominate.Services.IRepositories;
using Microsoft.AspNetCore.Http;
using System;

namespace Dominate.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
                throw ;
            }
        }
        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            using (var scope = context.RequestServices.CreateScope())
            {
                var routeData = context.GetRouteData();
                var _exceptionLoggerServices = scope.ServiceProvider.GetRequiredService<IExceptionLoggerServices>();
                var exception = new Exceptions
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Timestamp = DateTime.UtcNow,
                    ScreenName = routeData.Values["controller"]?.ToString() + "/" + routeData.Values["action"]?.ToString(),
                };
                _exceptionLoggerServices.InsertErrorLog(exception);
            }

        }
    }
}
