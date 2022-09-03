using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Models;

namespace StudyPlat.Message
{
    public class BookData
    {
        public string isbn { get; set;}
        public string book_name { get; set; }
        public string author { get; set; }
        public string publisher { get; set; }
        public DateTime? publish_time { get; set; }
        public string comprehension { get; set; }
        public string pic_url { get; set; }
    }
    public class BookMessage
    {
        public Header header { get; set; }
        public BookData data { get; set; }

    }
}
