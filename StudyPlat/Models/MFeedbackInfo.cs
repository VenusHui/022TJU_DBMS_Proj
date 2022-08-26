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
            _context = context;
        }
        public int AddFeedBack(string content)
        {
            FeedbackInfo feedbackInfo = new FeedbackInfo
            {
                Content = content,
                PostTime = DateTime.Now,
                IsFinished = false
            };
            try
            {
                _context.Add(feedbackInfo);
                _context.SaveChanges();
                return 0;
            }
            catch
            {
                return -1;//数据库有问题
            }
        }
    }
}
