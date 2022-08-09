using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class FeedbackInfo
    {
        public string FeedbackId { get; set; }
        public bool? ProblemType { get; set; }
        public string Content { get; set; }
        public DateTime PostTime { get; set; }
        public string Replay { get; set; }
        public string QuestionId { get; set; }

        public virtual Question Question { get; set; }
        public virtual FeedbackPosting FeedbackPosting { get; set; }
        public virtual FeedbackReception FeedbackReception { get; set; }
    }
}
