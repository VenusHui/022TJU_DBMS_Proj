using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class Question
    {
        public Question()
        {
            CollectionQuestion = new HashSet<CollectionQuestion>();
            ExplainQuestion = new HashSet<ExplainQuestion>();
            QuestionFromCourse = new HashSet<QuestionFromCourse>();
        }

        public string QuestionId { get; set; }
        public string QuestionStem { get; set; }
        public bool Status { get; set; }
        public string Source { get; set; }
        public DateTime PostTime { get; set; }
        public string PicUrl { get; set; }

        public virtual QuestionFromBook QuestionFromBook { get; set; }
        public virtual ICollection<CollectionQuestion> CollectionQuestion { get; set; }
        public virtual ICollection<ExplainQuestion> ExplainQuestion { get; set; }
        public virtual ICollection<QuestionFromCourse> QuestionFromCourse { get; set; }
    }
}
