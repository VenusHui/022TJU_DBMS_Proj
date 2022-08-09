using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entitys
{
    public partial class User
    {
        public User()
        {
            CollectionBook = new HashSet<CollectionBook>();
            CollectionCourse = new HashSet<CollectionCourse>();
            CollectionQuestion = new HashSet<CollectionQuestion>();
            FeedbackPosting = new HashSet<FeedbackPosting>();
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? UserType { get; set; }
        public string PhoneNumbe { get; set; }

        public virtual ICollection<CollectionBook> CollectionBook { get; set; }
        public virtual ICollection<CollectionCourse> CollectionCourse { get; set; }
        public virtual ICollection<CollectionQuestion> CollectionQuestion { get; set; }
        public virtual ICollection<FeedbackPosting> FeedbackPosting { get; set; }
    }
}
