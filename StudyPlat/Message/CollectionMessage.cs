using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPlat.Message
{
    public class CollectionData
    {
        public string[] idArray { get; set; }
    }
    public class CollectionMessage
    {
        public Header header { get; set; }

        public CollectionData data { get; set; }
    }
}
