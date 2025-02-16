﻿using AutoMapper;
using UniversityPilot.BLL.Areas.Schedule.Interfaces;
using UniversityPilot.BLL.Areas.Schedule.Models;
using UniversityPilot.DAL.Areas.AcademicCalendar.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared.Enumes;
using UniversityPilot.DAL.Areas.StudyOrganization.Interfaces;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.BLL.Areas.Schedule.Services
{
    public class GroupsScheduleService : IGroupsScheduleService
    {
        private readonly IMapper _mapper;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IScheduleClassDayRepository _scheduleClassDayRepository;
        private readonly ICourseRepository _courseRepository;

        public GroupsScheduleService(
            IMapper mapper,
            ISemesterRepository semesterRepository,
            IScheduleClassDayRepository scheduleClassDayRepository,
            ICourseRepository courseRepository)
        {
            _mapper = mapper;
            _semesterRepository = semesterRepository;
            _scheduleClassDayRepository = scheduleClassDayRepository;
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<SemesterDTO>> GetUpcomingSemestersAsync(int count = 3)
        {
            return _mapper.Map<List<SemesterDTO>>(await _semesterRepository.GetUpcomingSemestersAsync(count));
        }

        public async Task<FieldsOfStudyAssignmentDto> GetFieldsOfStudyAssignmentsToGroupAsync(int semesterId)
        {
            var scheduleClassDays = await _scheduleClassDayRepository.GetBySemesterIdAsync(semesterId);

            foreach (var scd in scheduleClassDays)
            {
                scd.StudyPrograms = scd.StudyPrograms
                    .Where(sp => sp.StudyForm == StudyForms.PartTimeWeekend
                              || sp.StudyForm == StudyForms.PartTimeWeekendOnline)
                    .ToList();
            }

            var assignedPrograms = scheduleClassDays
                .SelectMany(scd => scd.StudyPrograms)
                .DistinctBy(sp => sp.Id)
                .ToList();

            var allProgramsFromCourses = await _courseRepository.GetStudyProgramsBySemesterIdAsync(semesterId);

            var relevantPrograms = allProgramsFromCourses
                .Where(sp => sp.StudyForm == StudyForms.PartTimeWeekend
                          || sp.StudyForm == StudyForms.PartTimeWeekendOnline)
                .ToList();

            var unassignedPrograms = relevantPrograms
                .Where(sp => !assignedPrograms.Any(ap => ap.Id == sp.Id))
                .ToList();

            var unassignedFieldsOfStudy = unassignedPrograms
                .Select(sp => new
                {
                    Program = sp,
                    Formatted = FormatFieldOfStudy(sp),
                    Rank = GetStudyDegreeRank(sp.StudyDegree),
                    NameOnly = sp.FieldOfStudy.Name
                })
                .DistinctBy(x => x.Formatted)
                .OrderBy(x => x.Rank)
                .ThenBy(x => x.NameOnly)
                .Select(x => x.Formatted)
                .ToList();

            var assignedFieldOfStudyGroups = scheduleClassDays
                .Select(scd => new FieldOfStudyGroupDto
                {
                    GroupId = scd.Id,
                    GroupName = scd.Title,
                    AssignedFieldsOfStudy = scd.StudyPrograms
                        .Select(sp => new
                        {
                            Program = sp,
                            Formatted = FormatFieldOfStudy(sp),
                            Rank = GetStudyDegreeRank(sp.StudyDegree),
                            NameOnly = sp.FieldOfStudy.Name
                        })
                        .DistinctBy(x => x.Formatted)
                        .OrderBy(x => x.Rank)
                        .ThenBy(x => x.NameOnly)
                        .Select(x => x.Formatted)
                        .ToList()
                })
                .ToList();

            return new FieldsOfStudyAssignmentDto
            {
                SemesterId = semesterId,
                UnassignedFieldsOfStudy = unassignedFieldsOfStudy,
                AssignedFieldOfStudyGroups = assignedFieldOfStudyGroups
            };
        }

        public async Task UpdateFieldsOfStudyAssignmentsToGroupAsync(FieldsOfStudyAssignmentDto model)
        {
            var scheduleClassDaysInDb = await _scheduleClassDayRepository.GetBySemesterIdAsync(model.SemesterId);
            var relevantPrograms = await _courseRepository.GetStudyProgramsBySemesterIdAsync(model.SemesterId);

            var relevantProgramsFiltered = relevantPrograms
                .Where(sp => sp.StudyForm == StudyForms.PartTimeWeekend
                          || sp.StudyForm == StudyForms.PartTimeWeekendOnline)
                .ToList();

            var stringToProgram = relevantProgramsFiltered
                .Select(sp => new
                {
                    Program = sp,
                    Formatted = FormatFieldOfStudy(sp)
                })
                .DistinctBy(x => x.Formatted, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    x => x.Formatted,
                    x => x.Program,
                    StringComparer.OrdinalIgnoreCase
                );

            foreach (var scd in scheduleClassDaysInDb)
            {
                scd.StudyPrograms.Clear();
            }

            var frontExistingGroupIds = model.AssignedFieldOfStudyGroups
                .Where(g => g.GroupId != 0)
                .Select(g => g.GroupId)
                .ToList();

            var groupsToRemove = scheduleClassDaysInDb
                .Where(scd => !frontExistingGroupIds.Contains(scd.Id))
                .ToList();

            foreach (var groupToRemove in groupsToRemove)
            {
                await _scheduleClassDayRepository.DeleteAsync(groupToRemove);
            }

            foreach (var groupDto in model.AssignedFieldOfStudyGroups)
            {
                ScheduleClassDay scd;
                if (groupDto.GroupId == 0)
                {
                    scd = new ScheduleClassDay
                    {
                        Title = groupDto.GroupName,
                        SemesterId = model.SemesterId
                    };
                    await _scheduleClassDayRepository.AddAsync(scd);
                }
                else
                {
                    scd = scheduleClassDaysInDb
                        .First(x => x.Id == groupDto.GroupId);

                    if (scd == null) continue;

                    scd.Title = groupDto.GroupName;
                }

                foreach (var assignedName in groupDto.AssignedFieldsOfStudy)
                {
                    if (stringToProgram.TryGetValue(assignedName, out var program) &&
                        !scd.StudyPrograms.Contains(program))
                    {
                        scd.StudyPrograms.Add(program);
                    }
                }
            }

            await _scheduleClassDayRepository.SaveChangesAsync();
        }

        private static string FormatFieldOfStudy(StudyProgram sp)
        {
            var baseName = sp.FieldOfStudy.Name;
            var suffix = sp.StudyDegree switch
            {
                StudyDegree.Inz => " - I Stopień",
                StudyDegree.Lic => " - I Stopień",
                StudyDegree.Mgr => " - II Stopień",
                StudyDegree.USM => " - II Stopień",
                StudyDegree.USM3Sem => " - II Stopień",
                StudyDegree.USMSp => " - II Stopień",
                _ => " - Nieznany Stopień"
            };

            return baseName + suffix;
        }

        private static int GetStudyDegreeRank(StudyDegree sd)
        {
            return sd switch
            {
                StudyDegree.Inz => 0,
                StudyDegree.Lic => 0,
                StudyDegree.Mgr => 1,
                StudyDegree.USM => 1,
                StudyDegree.USM3Sem => 1,
                StudyDegree.USMSp => 1,
                _ => 2
            };
        }
    }
}