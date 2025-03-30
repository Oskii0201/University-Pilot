using Microsoft.Extensions.DependencyInjection;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.AcademicCalendar.Repositories;
using UniversityPilot.DAL.Areas.Identity.Interfaces;
using UniversityPilot.DAL.Areas.Identity.Repositories;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Repositories;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Repositories;

namespace UniversityPilot.DAL
{
    public static class RepositoriesRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            #region AcademicCalendar

            services.AddScoped<ISemesterRepository, SemesterRepository>();
            services.AddScoped<IHolidayRepository, HolidayRepository>();

            #endregion AcademicCalendar

            #region Identity

            services.AddScoped<IAccountRepostiory, AccountRepostiory>();

            #endregion Identity

            #region SemesterPlanning

            services.AddScoped<IClassDayRepository, ClassDayRepository>();
            services.AddScoped<IScheduleClassDayRepository, ScheduleClassDayRepository>();
            services.AddScoped<IScheduleClassDayStudyProgramRepository, ScheduleClassDayStudyProgramRepository>();

            #endregion SemesterPlanning

            #region Shared

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            #endregion Shared

            #region StudyOrganization

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseDetailsRepository, CourseDetailsRepository>();
            services.AddScoped<IFieldOfStudyRepository, FieldOfStudyRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<IStudyProgramRepository, StudyProgramRepository>();

            #endregion StudyOrganization
        }
    }
}