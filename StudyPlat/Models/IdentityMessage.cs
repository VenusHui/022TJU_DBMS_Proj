using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPlat.Models
{
    public class data
    {
        public int user_type { get; set; }
        public string token { get; set; }
    }
    public class IdentityMessage
    {
        public int code { get; set; }
        public string message { get; set; }

        public data data { get; set; }
    }
}
