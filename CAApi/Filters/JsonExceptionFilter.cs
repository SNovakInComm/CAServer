using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using CAApi.Models;

namespace CAApi.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        private IHostingEnvironment _hostingEnvironment;

        public JsonExceptionFilter(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
        }

        public void OnException(ExceptionContext context)
        {
            var error = new ApiError();

            if (_hostingEnvironment.IsDevelopment())
            {
                error.Message = context.Exception.Message;
                error.Detail = context.Exception.StackTrace;
            }
            else
            {
                error.Message = "A Server Error Occurred.";
            }

            context.Result = new ObjectResult(error)
            {
                StatusCode = 500
            };
        }
    }
}
