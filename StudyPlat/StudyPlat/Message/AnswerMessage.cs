using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Message;

namespace StudyPlat.Message
{
    public class AnswerData
    {
        public string answer_id { get; set; }

        public string answer_content { get; set; }
        public string expert_name { get; set; }
        public decimal? approve { set; get; }
    }
    public class AnswerMessage
    {
        public Header header { get; set; } 
        public AnswerData data { get; set; }
    }
}
