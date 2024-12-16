﻿using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using SkillMiner.Application.Shared.Results;
using SkillMiner.Domain.Shared.BusinessRule;
using SkillMiner.Domain.Shared.Errors;
using SkillMiner.Presentation.Web.Serialization;

namespace SkillMiner.Presentation.Web.Middleware;

/// <summary>
/// Extension methods for configuring exception handling in an ASP.NET Core application.
/// </summary>
public static class ExceptionMiddleware
{
    /// <summary>
    /// Configures exception handling in the application by adding an exception handler middleware to the request pipeline.
    /// </summary>
    /// <param name="app">The ASP.NET Core application instance.</param>
    public static void ConfigureExceptionHander(this WebApplication app)
    {
        // UseExceptionHandler adds a terminal middleware delegate to the application's request pipeline.
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                IExceptionHandlerFeature? contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature is null)
                {
                    return;
                }

                // Set the response content type to JSON.
                context.Response.ContentType = "application/json; charset=utf-8";

                Result? result = null;

                if (contextFeature.Error is ErrorException ErrorException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    result = Result.Failure(ErrorException.Errors);
                }
                else if (contextFeature.Error is BusinessRuleBrokenException businessRuleBrokenExeption)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    result = Result.Failure(new Error(businessRuleBrokenExeption.Code, businessRuleBrokenExeption.Message));
                }
                else
                {
                    // Set the response status code to 500 (Internal Server Error).
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    result = Result.Failure(new Error("InternalServerError", contextFeature.Error.Message));
                }

                // Serialize a Result instance with the error information.
                string response = JsonConvert.SerializeObject(result, JsonSerialization.Options);

                // Write the JSON response to the response stream.
                await context.Response.WriteAsync(response);
            });
        });
    }
}