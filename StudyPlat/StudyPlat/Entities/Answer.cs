﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class Answer
    {
        public Answer()
        {
            ExplainQuestion = new HashSet<ExplainQuestion>();
            GiveAnswer = new HashSet<GiveAnswer>();
            UserApproveAnswer = new HashSet<UserApproveAnswer>();
        }

        public string AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public decimal Approve { get; set; }

        public virtual ICollection<ExplainQuestion> ExplainQuestion { get; set; }
        public virtual ICollection<GiveAnswer> GiveAnswer { get; set; }
        public virtual ICollection<UserApproveAnswer> UserApproveAnswer { get; set; }
    }
}