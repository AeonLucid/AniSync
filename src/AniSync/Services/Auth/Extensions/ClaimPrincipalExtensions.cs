using System.Security.Claims;

namespace AniSync.Services.Auth.Extensions
{
    public static class ClaimPrincipalExtensions
    {
        public static string GetClaim(this ClaimsPrincipal principal, AuthClaim claim)
        {
            return principal.FindFirstValue(claim.ToString());
        }
    }
}
