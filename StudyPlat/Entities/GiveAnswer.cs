using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class GiveAnswer
    {
        public string ExpertId { get; set; }
        public string AnswerId { get; set; }
        public DateTime? AdditionDate { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Expert Expert { get; set; }
    }
}
