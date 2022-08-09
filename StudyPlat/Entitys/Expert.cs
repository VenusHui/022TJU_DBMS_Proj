using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entitys
{
    public partial class Expert
    {
        public Expert()
        {
            GiveAnswer = new HashSet<GiveAnswer>();
            HasExpert = new HashSet<HasExpert>();
        }

        public string ExpertId { get; set; }
        public string ExpertName { get; set; }

        public virtual ICollection<GiveAnswer> GiveAnswer { get; set; }
        public virtual ICollection<HasExpert> HasExpert { get; set; }
    }
}
