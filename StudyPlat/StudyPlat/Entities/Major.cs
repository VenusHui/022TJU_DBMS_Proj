using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class Major
    {
        public Major()
        {
            CourseFromMajor = new HashSet<CourseFromMajor>();
            HasExpert = new HashSet<HasExpert>();
            QuestionFromMajor = new HashSet<QuestionFromMajor>();
        }

        public string MajorId { get; set; }
        public string MajorName { get; set; }

        public virtual ICollection<CourseFromMajor> CourseFromMajor { get; set; }
        public virtual ICollection<HasExpert> HasExpert { get; set; }
        public virtual ICollection<QuestionFromMajor> QuestionFromMajor { get; set; }
    }
}
