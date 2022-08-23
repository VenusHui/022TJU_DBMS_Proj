using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class Course
    {
        public Course()
        {
            CollectionCourse = new HashSet<CollectionCourse>();
            CourseFromMajor = new HashSet<CourseFromMajor>();
            HasBook = new HashSet<HasBook>();
            QuestionFromCourse = new HashSet<QuestionFromCourse>();
        }

        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string Comprehension { get; set; }
        public string PicUrl { get; set; }

        public virtual ICollection<CollectionCourse> CollectionCourse { get; set; }
        public virtual ICollection<CourseFromMajor> CourseFromMajor { get; set; }
        public virtual ICollection<HasBook> HasBook { get; set; }
        public virtual ICollection<QuestionFromCourse> QuestionFromCourse { get; set; }
    }
}
