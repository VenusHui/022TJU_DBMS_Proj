using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entitys
{
    public partial class FeedbackPosting
    {
        public string FeedbackId { get; set; }
        public string UserId { get; set; }

        public virtual FeedbackInfo Feedback { get; set; }
        public virtual User User { get; set; }
    }
}
