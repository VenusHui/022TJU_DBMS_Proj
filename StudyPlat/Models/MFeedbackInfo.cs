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
        public FeedbackInfo GetFeedBack(string feedback_id)
        {
            IQueryable<FeedbackInfo> feedbackInfos = _context.FeedbackInfo;
            feedbackInfos = feedbackInfos.Where(u => u.FeedbackId == feedback_id);
            int num = feedbackInfos.Count();
            if(num == 1)
            {
                return feedbackInfos.First();
            }
            else
            {
                return new FeedbackInfo
                {
                    FeedbackId = "-1",
                    PostTime = DateTime.Now
                };
            }
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

        public List<string> FindUnfinished()
        {
            List<string> UnfinishedList = new List<string> { };
            IQueryable<FeedbackInfo> feedbackInfos = _context.FeedbackInfo;
            feedbackInfos = feedbackInfos.Where(u => u.IsFinished == false);
            foreach(var row in feedbackInfos)
            {
                UnfinishedList.Add(row.FeedbackId);
            }
            return UnfinishedList;
        }

        public List<string> FindFinished()
        {
            List<string> FinishedList = new List<string> { };
            IQueryable<FeedbackInfo> feedbackInfos = _context.FeedbackInfo;
            feedbackInfos = feedbackInfos.Where(u => u.IsFinished == true);
            foreach(var row in feedbackInfos)
            {
                FinishedList.Add(row.FeedbackId);
            }
            return FinishedList;
        }

        public int SwitchStatus(string feedback_id)
        {
            IQueryable<FeedbackInfo> feedbackInfos = _context.FeedbackInfo;
            feedbackInfos = feedbackInfos.Where(u => u.FeedbackId == feedback_id);
            int num = feedbackInfos.Count();
            if(num == 1)
            {
                FeedbackInfo temp = feedbackInfos.First();
                if (temp.IsFinished == true)
                    temp.IsFinished = false;
                else
                    temp.IsFinished = true;
                try
                {
                    _context.FeedbackInfo.Update(temp);
                    _context.SaveChanges();
                    return 0;
                }
                catch
                {
                    return -1;//数据库操作出现问题
                }
            }
            else
            {
                return -2;//说明没找到对应的反馈消息
            }
        }
    }
}
