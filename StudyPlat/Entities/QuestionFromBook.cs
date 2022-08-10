using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class QuestionFromBook
    {
        public string QuestionId { get; set; }
        public string Isbn { get; set; }
        public short? Page { get; set; }

        public virtual Book IsbnNavigation { get; set; }
        public virtual Question Question { get; set; }
    }
}
