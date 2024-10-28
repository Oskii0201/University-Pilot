using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UniversityPilot.BLL.Areas.Identity.Interfaces;

namespace UniversityPilot.BLL.Areas.Identity.Services
{
    internal class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

        public int GetUserId =>
            User is null ? 0 : int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public string GetRoleName =>
            User is null ? "" : User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
    }
}