using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Message;

namespace StudyPlat.Message
{
    public  class QueryData
    {
        public List<string> IdList { get; set; }
    }
    public class QueryMessage
    {
        public Header header { get; set; }

        public QueryData data { get; set; }

    }
    public class RecommendData
    {
        public List<string> isbnList { set; get; }
        public List<string> questionIDList { set; get; }
    }
    public class RecommendMessage
    {
        public Header header { get; set; }
        public RecommendData data { get; set; }
    }
}
