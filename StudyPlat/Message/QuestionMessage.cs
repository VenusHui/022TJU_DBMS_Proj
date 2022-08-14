using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPlat.Message
{
    public class QuestionData
    {
        public string pic_url { get; set; }
        public string question_stem { get; set; }
        public string[] answer_id_list { get; set; }
        public string source { get; set; }
        public DateTime post_time { get; set; }
    }
    public class QuestionMessage
    {
        public QuestionData data { get; set; }
        public Header header { get; set; }
    }
}
