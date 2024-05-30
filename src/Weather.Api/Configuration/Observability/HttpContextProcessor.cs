using System.Diagnostics;
using OpenTelemetry;

namespace Weather.Api.Configuration.Observability;

internal sealed class HttpContextProcessor(IHttpContextAccessor httpContextAccessor) : BaseProcessor<Activity>
{
    public override void OnStart(Activity data)
    {
        var username = httpContextAccessor.HttpContext?.User.Identity?.Name;
        if(username is not null) data.SetTag(nameof(username), username);
        
        base.OnStart(data);
    }
}