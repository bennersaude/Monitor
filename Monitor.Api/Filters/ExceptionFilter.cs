using System;
using System.Linq;
using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Monitor.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!context.HttpContext.Request.Path.Value.Contains("/swagger"))
                return;

            HttpStatusCode status = HttpStatusCode.InternalServerError;
            var message = context.Exception.Message;

            if (context.Exception is ValidationException)
            {
                message = String.Join("\r\n- ", (context.Exception as ValidationException).Errors.Select(x => x.ErrorMessage));
            }

            context.ExceptionHandled = true;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            context.Result = new ObjectResult(new { error = message });
        }
    }
}