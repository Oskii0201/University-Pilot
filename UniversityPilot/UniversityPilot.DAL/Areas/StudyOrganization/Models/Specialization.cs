﻿using UniversityPilot.DAL.Areas.Shared;

namespace UniversityPilot.DAL.Areas.StudyOrganization.Models
{
    public class Specialization : IModelBase
    {
        public Specialization()
        {
            Courses = new HashSet<Course>();
            StudyPrograms = new HashSet<StudyProgram>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<StudyProgram> StudyPrograms { get; set; }
    }
}