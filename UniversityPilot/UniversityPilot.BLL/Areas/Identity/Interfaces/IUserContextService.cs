using System.Security.Claims;

namespace UniversityPilot.BLL.Areas.Identity.Interfaces
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        int GetUserId { get; }
        public string GetRoleName { get; }
    }
}