using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;
using StudyPlat.Message;

namespace StudyPlat.Models
{
    public class MFeedbackInfo : FeedbackInfo
    {
        public readonly ModelContext _context;
        public MFeedbackInfo(ModelContext context)
        {

        }
    }
}
