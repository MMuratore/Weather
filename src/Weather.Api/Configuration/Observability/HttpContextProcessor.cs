using System.Diagnostics;
using System.Net;
using System.Text.Json;
using OpenTelemetry;

namespace Weather.Api.Configuration.Observability;

internal sealed class HttpContextProcessor(IHttpContextAccessor httpContextAccessor) : BaseProcessor<Activity>
{
    public override void OnStart(Activity data)
    {
        SetUsernameTag(data);

        base.OnStart(data);
    }

    private void SetUsernameTag(Activity data)
    {
        var username = httpContextAccessor.HttpContext?.User.Identity?.Name;
        if (username is not null) data.SetTag(nameof(username), username);
    }

    public override void OnEnd(Activity data)
    {
        SetTagsInCaseOfHttpError(data);

        base.OnEnd(data);
    }

    private void SetTagsInCaseOfHttpError(Activity data)
    {
        var response = httpContextAccessor.HttpContext?.Response;
        if (response is null) return;

        if (response.StatusCode < (int)HttpStatusCode.BadRequest) return;
        
        data.SetTag(nameof(HttpRequest), httpContextAccessor.HttpContext?.Request);
        data.SetTag(nameof(HttpResponse), response);
    }
}