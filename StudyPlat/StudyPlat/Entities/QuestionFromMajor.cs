using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class QuestionFromMajor
    {
        public string QuestionId { get; set; }
        public string MajorId { get; set; }

        public virtual Major Major { get; set; }
        public virtual Question Question { get; set; }
    }
}
