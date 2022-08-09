using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace StudyPlat.Entitys
{
    public partial class Administrator
    {
        public Administrator()
        {
            FeedbackReception = new HashSet<FeedbackReception>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecondaryP { get; set; }
        public string PhoneNumbe { get; set; }
        public string Position { get; set; }

        public virtual ICollection<FeedbackReception> FeedbackReception { get; set; }
    }
}
