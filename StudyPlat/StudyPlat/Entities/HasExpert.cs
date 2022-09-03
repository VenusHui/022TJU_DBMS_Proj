using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class HasExpert
    {
        public string MajorId { get; set; }
        public string ExpertId { get; set; }

        public virtual User Expert { get; set; }
        public virtual Major Major { get; set; }
    }
}
