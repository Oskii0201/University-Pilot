﻿using UniversityPilot.DAL.Areas.Identity.Models;
using UniversityPilot.DAL.Areas.SemesterPlanning.Models;
using UniversityPilot.DAL.Areas.StudyOrganization.Models;

namespace UniversityPilot.DAL.Areas.UniversityComponents.Models
{
    public class Instructor : User
    {
        public Instructor() : base()
        {
            CourseSchedules = new HashSet<CourseSchedule>();
            Courses = new HashSet<Course>();
        }

        public string ContractType { get; set; }

        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}