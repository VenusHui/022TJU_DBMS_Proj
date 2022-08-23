using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPlat.Message
{
    public class ExpertData
    {
        public List<string> finishedIDList { get; set; }
        public List<string> unfinishedIDList { get; set; }
    }

    public class ExpertMessage
    {
        public Header header { get; set; }
        public ExpertData data { get; set; }
    }
}
