using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;

namespace StudyPlat.Message
{
    public class MajorInfo
    {
        public List<string> IDList { get; set; }
        public List<string> NameList { get; set; }
    }

    public class MajorMessage
    {
        public Header header { get; set; }
        public MajorInfo data { get; set; }
    }
}
