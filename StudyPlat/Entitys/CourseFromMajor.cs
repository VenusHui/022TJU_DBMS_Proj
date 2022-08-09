using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entitys
{
    public partial class CourseFromMajor
    {
        public string MajorId { get; set; }
        public string CourseId { get; set; }
        public string Type { get; set; }

        public virtual Course Course { get; set; }
        public virtual Major Major { get; set; }
    }
}
