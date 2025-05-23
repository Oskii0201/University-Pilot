﻿using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UniversityPilot.BLL.Areas.Files.Interfaces;
using UniversityPilot.BLL.Areas.Files.Services;
using UniversityPilot.BLL.Areas.Identity.DTO;
using UniversityPilot.BLL.Areas.Identity.Interfaces;
using UniversityPilot.BLL.Areas.Identity.Services;
using UniversityPilot.BLL.Areas.Identity.Validators;
using UniversityPilot.BLL.Areas.Processing.Interfaces;
using UniversityPilot.BLL.Areas.Processing.Services;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Services;
using UniversityPilot.DAL.Areas.Identity.Models;

namespace UniversityPilot.BLL
{
    public static class ServicesRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            #region Identity

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();

            #endregion Identity

            #region Files

            services.AddScoped<ICsvService, CsvService>();
            services.AddScoped<IFileService, FileService>();

            #endregion Files

            #region Processing

            services.AddScoped<IClassroomService, ClassroomService>();
            services.AddScoped<ICourseDetailsService, CourseDetailsService>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IStudyProgramService, StudyProgramService>();
            services.AddScoped<IInstructorService, InstructorService>();

            #endregion Processing

            #region Schedule

            services.AddScoped<IGroupsScheduleService, GroupsScheduleService>();
            services.AddScoped<IScheduleGenerator, ScheduleGenerator>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ISemesterService, SemesterService>();

            #endregion Schedule
        }
    }
}