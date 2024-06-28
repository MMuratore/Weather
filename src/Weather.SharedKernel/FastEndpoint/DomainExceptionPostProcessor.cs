using System.Net;
using FastEndpoints;
using Weather.SharedKernel.Exception;

namespace Weather.SharedKernel.FastEndpoint;

public sealed class DomainExceptionPostProcessor : IGlobalPostProcessor
{
    public async Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        if (!context.HasExceptionOccurred)
            return;

        var domainException = context.ExceptionDispatchInfo.SourceException as DomainException;
        if (domainException is null) context.ExceptionDispatchInfo.Throw();

        context.MarkExceptionAsHandled();

        var status = domainException switch
        {
            ConflictException => HttpStatusCode.Conflict,
            ForbiddenException => HttpStatusCode.Forbidden,
            NotFoundException => HttpStatusCode.NotFound,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            ValidationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        await context.HttpContext.Response.SendAsync(domainException.Message, (int)status, cancellation: ct);
    }
}