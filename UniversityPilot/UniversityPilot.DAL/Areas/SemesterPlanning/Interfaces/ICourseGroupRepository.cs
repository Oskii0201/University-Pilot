﻿using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.Shared;
using UniversityPilot.DAL.Areas.Shared.Enumes;

namespace UniversityPilot.DAL.Areas.SemesterPlanning.Interfaces
{
    public interface ICourseGroupRepository : IRepository<CourseGroup>
    {
        public Task<List<CourseGroup>> GetByNamesAndTypesAsync(IEnumerable<(string Name, CourseTypes Type)> descriptors);
    }
}