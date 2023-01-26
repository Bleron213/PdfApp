﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PdfApp.Contracts.Response;
using PdfApp.Infrastructure.Errors;
using PdfApp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Infrastructure.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _hostingEnv;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment hostingEnv)
        {
            _next = next;
            _logger = logger;
            _hostingEnv = hostingEnv;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Error in Custom Middleware");

            var response = new Response<string>();
            if (_hostingEnv.IsDevelopment())
            {
                response.Errors.Add(new KeyValuePair<string, string>("Exception", exception.ToString()));
            }

            context.Response.ContentType = "application/json";

            if (exception is CustomError)
            {
                var customError = exception as CustomError;

                response.Message = "Custom Error ocurred";
                response.Errors.AddRange(customError.SerializeErrors());
                response.StatusCode = (int)HttpStatusCode.BadRequest;

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(response.ToString());
                return;
            }

            response.Message = "Unexpected error ocurred";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(response.ToString());

        }
    }
}