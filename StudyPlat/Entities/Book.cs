using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class Book
    {
        public Book()
        {
            CollectionBook = new HashSet<CollectionBook>();
            HasBook = new HashSet<HasBook>();
            QuestionFromBook = new HashSet<QuestionFromBook>();
        }

        public string Isbn { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public byte? PublishTime { get; set; }
        public string Comprehension { get; set; }
        public string PicUrl { get; set; }

        public virtual ICollection<CollectionBook> CollectionBook { get; set; }
        public virtual ICollection<HasBook> HasBook { get; set; }
        public virtual ICollection<QuestionFromBook> QuestionFromBook { get; set; }
    }
}
