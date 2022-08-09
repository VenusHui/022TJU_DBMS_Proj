using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entitys
{
    public partial class CollectionBook
    {
        public string Isbn { get; set; }
        public string UserId { get; set; }
        public string Note { get; set; }
        public DateTime? CollectTime { get; set; }

        public virtual Book IsbnNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
