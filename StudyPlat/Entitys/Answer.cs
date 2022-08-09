using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entitys
{
    public partial class Answer
    {
        public Answer()
        {
            ExplainQuestion = new HashSet<ExplainQuestion>();
            GiveAnswer = new HashSet<GiveAnswer>();
        }

        public string AnswerId { get; set; }
        public string AnswerContent { get; set; }

        public virtual ICollection<ExplainQuestion> ExplainQuestion { get; set; }
        public virtual ICollection<GiveAnswer> GiveAnswer { get; set; }
    }
}
