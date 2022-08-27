using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPlat.Message
{
    public class GetFeedbackData
    {
        public List<string> finishedList { set; get; }
        public List<string> unfinishedList { set; get; }
    }
    public class FeedbackData
    {
        public string feedback_id { set; get; }
        public string content { set; get; }
        public DateTime time { set; get; }
        public bool? isfinished { set; get; }
    }
    public class FeedbackMessage
    {
        public Header header { set; get; }
        public FeedbackData data { set; get; }
    }

    public class GetFeedbackMessage
    {
        public Header header { set; get; }
        public GetFeedbackData data { set; get; }
    }
}
