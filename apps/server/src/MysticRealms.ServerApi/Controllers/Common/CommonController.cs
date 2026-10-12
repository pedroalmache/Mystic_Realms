using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace MysticRealms.ServerApi.Controllers.Common;

public record VersionResponse(string Version);

[ApiController]
[Route("api/[controller]")]
public class CommonController : ControllerBase
{
    [HttpGet("version")]
    public ActionResult<VersionResponse> GetVersion()
    {
        var version = Assembly.GetEntryAssembly()
            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion
            ?? "1.0.0";

        return Ok(new VersionResponse(version));
    }
}
