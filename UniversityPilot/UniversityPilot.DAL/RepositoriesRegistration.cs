using Microsoft.Extensions.DependencyInjection;
using UniversityPilot.DAL.Areas.Identity.Interfaces;
using UniversityPilot.DAL.Areas.Identity.Repositories;
using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL
{
    public static class RepositoriesRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            #region Identity

            services.AddScoped<IAccountRepostiory, AccountRepostiory>();

            #endregion Identity

            #region Shared

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            #endregion Shared
        }
    }
}