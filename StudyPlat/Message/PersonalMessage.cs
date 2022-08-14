using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPlat.Message
{
    public class PersonalInformation
    {
        public string user_name { get; set; }
        public string school { get; set; }
        public string major_id { get; set; }
    }
    public class PersonalMessage
    {
        public Header header { get; set; }
        public PersonalInformation personalInformation { get; set; }
    }
}
