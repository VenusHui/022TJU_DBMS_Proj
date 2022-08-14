using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPlat.Message
{

    public class CourseData
    {
        public string course_id { get; set; }
        public string course_name { get; set; }
        public string comprehension { get; set; }
    }
    public class CourseMessage
    {
        public Header header { get; set; }
        public CourseData data { get; set; }
    }
}
