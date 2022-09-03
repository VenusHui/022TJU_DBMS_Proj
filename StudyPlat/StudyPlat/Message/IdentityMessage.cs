using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPlat.Message
{
    public class UserInfo
    {
        public int user_type { get; set; }
        public string user_id { get; set; }
    }
    public class IdentityData
    {
        public UserInfo user { get; set; }
        public string token { get; set; }
    }


    public class IdentityMessage
    {
        public IdentityData data { get; set; }

        public Header header { get; set; }
    }
}
