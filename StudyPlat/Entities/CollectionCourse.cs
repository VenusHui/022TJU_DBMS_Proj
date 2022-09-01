using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class CollectionCourse
    {
        public string CourseId { get; set; }
        public string UserId { get; set; }
        public DateTime CollectTime { get; set; }

        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
    }
}
