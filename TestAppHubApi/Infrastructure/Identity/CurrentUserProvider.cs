using System;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using TestApp.Domain;

namespace TestAppApi.Infrastructure.Identity
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long GetUserId()
        {
            foreach (var claim in User.Claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    return long.Parse(claim.Value);
                }
            }

            throw new InvalidOperationException("Claim does not exist!");
        }

        private ClaimsPrincipal User => _httpContextAccessor.HttpContext.User;
    }
}
