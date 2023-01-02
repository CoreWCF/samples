using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Service
{
    [Authorize]
    public partial class SecuredService : ISecuredService
    {
        public string Echo(string value, [FromServices] HttpContext httpContext, [FromServices] ILogger<SecuredService> logger)
        {
            var principal = httpContext.User;
            logger.LogInformation("Principal has claims: {claims}",
                string.Join(", ", principal.Claims.Select(x => $"'{x.Type}'='{x.Value}'")));
            return value;
        }
    }
}
