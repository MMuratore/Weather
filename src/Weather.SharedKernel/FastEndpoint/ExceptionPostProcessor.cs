using System.Diagnostics;
using System.Net;
using FastEndpoints;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Weather.SharedKernel.Exception;

namespace Weather.SharedKernel.FastEndpoint;

public sealed class ExceptionPostProcessor(ILogger<ExceptionPostProcessor> logger) : IGlobalPostProcessor
{
    public async Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        if (!context.HasExceptionOccurred)
            return;

        context.MarkExceptionAsHandled();

        logger.LogError(context.ExceptionDispatchInfo.SourceException, context.ExceptionDispatchInfo.SourceException.Message);
        
        var domainException = context.ExceptionDispatchInfo.SourceException as DomainException;

        var status = domainException switch
        {
            ConflictException => HttpStatusCode.Conflict,
            ForbiddenException => HttpStatusCode.Forbidden,
            NotFoundException => HttpStatusCode.NotFound,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };
        
        var details = new ProblemDetails
        {
            Detail = context.ExceptionDispatchInfo.SourceException.Message,
            Instance = context.HttpContext.Request.Path,
            Status = (int)status,
            TraceId = context.HttpContext.TraceIdentifier,
        };
        
        if (domainException is not null)
        {
            details.Errors = 
            [
                new ProblemDetails.Error
            {
                Name = domainException.Property ?? context.ExceptionDispatchInfo.SourceException.GetType().Name,
                Code = domainException.Code,
                Reason = domainException.Message
            }];
        }
        
        await context.HttpContext.Response.SendAsync(details, (int)status, cancellation: ct);
    }
}