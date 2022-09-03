﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entities
{
    public partial class User
    {
        public User()
        {
            CollectionBook = new HashSet<CollectionBook>();
            CollectionCourse = new HashSet<CollectionCourse>();
            CollectionQuestion = new HashSet<CollectionQuestion>();
            GiveAnswer = new HashSet<GiveAnswer>();
            HasExpert = new HashSet<HasExpert>();
            UserApproveAnswer = new HashSet<UserApproveAnswer>();
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte UserType { get; set; }
        public string PhoneNumber { get; set; }
        public string SchoolName { get; set; }
        public string MajorId { get; set; }

        public virtual ICollection<CollectionBook> CollectionBook { get; set; }
        public virtual ICollection<CollectionCourse> CollectionCourse { get; set; }
        public virtual ICollection<CollectionQuestion> CollectionQuestion { get; set; }
        public virtual ICollection<GiveAnswer> GiveAnswer { get; set; }
        public virtual ICollection<HasExpert> HasExpert { get; set; }
        public virtual ICollection<UserApproveAnswer> UserApproveAnswer { get; set; }
    }
}