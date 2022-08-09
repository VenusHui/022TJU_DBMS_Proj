using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class FeedbackReception
    {
        public string FeedbackId { get; set; }
        public string AdministratorId { get; set; }
        public DateTime? ReceptTime { get; set; }
        public bool? Read { get; set; }

        public virtual Administrator Administrator { get; set; }
        public virtual FeedbackInfo Feedback { get; set; }
    }
}
