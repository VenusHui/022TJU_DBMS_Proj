using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;

namespace StudyPlat.Models
{
    public class MUser : User
    {
        public ModelContext _context;
        public MUser(ModelContext context)
        {
            _context = context;
            CollectionBook = new HashSet<CollectionBook>();
            CollectionCourse = new HashSet<CollectionCourse>();
            CollectionQuestion = new HashSet<CollectionQuestion>();
            FeedbackPosting = new HashSet<FeedbackPosting>();
        }

        public string GenerateId()
        {
            IQueryable<User> users = _context.User;
            int num = 0;
            num = users.Count() + 1;
            return num.ToString();
        }

        public User findUser(string id)
        {
            IQueryable<User> users = _context.User;
            users = users.Where(u => u.UserId == id);
            int num = users.Count();
            User user; 
            if(num == 1)
            {
                user = users.First();
            }
            else
            {
                user = new User
                {
                    UserId = "-1",
                    Password = "123456",
                    UserType = 0
                };
            }
            return user;
        }
        //找到还没回答的问题
        public List<string> FindUnfinishedQuestion(string major_id)
        {
            MQuestion mQuestion = new MQuestion(_context);
            IQueryable<QuestionFromMajor> questionFromMajors = _context.QuestionFromMajor;
            IQueryable<Question> questions = _context.Question;
            List<string> questionIDList = new List<string> { };
            questionFromMajors = questionFromMajors.Where(u => u.MajorId == major_id);
            int num = questionFromMajors.Count();
            if(num == 0)
            {
                return questionIDList;
            }
            List<QuestionFromMajor> questionFromMajors1 = questionFromMajors.ToList();
            for (int i=0;i < num; i++)
            {
                string question_id = questionFromMajors1[i].QuestionId;
                if(mQuestion.getStatus(question_id) == false)
                {
                    questionIDList.Add(question_id);
                }
            }
            return questionIDList;
        }

        //找到这个专家回答过的问题
        public List<string> FindFinishedQuestion(string expert_id)
        {
            MAnswer mAnswer = new MAnswer(_context);
            IQueryable<ExplainQuestion> explainQuestions = _context.ExplainQuestion;
            IQueryable<GiveAnswer> giveAnswers = _context.GiveAnswer;
            List<string> questionIDList = new List<string> { };
            giveAnswers = giveAnswers.Where(u => u.ExpertId == expert_id);
            int num = giveAnswers.Count();
            List<GiveAnswer> giveAnswers1 = giveAnswers.ToList();
            for(int i =0;i < num;i++)
            {
                string question_id = mAnswer.findQuestionIDFromAnswerID(giveAnswers1[i].AnswerId);
                questionIDList.Add(question_id);
            }
            return questionIDList;
        }

        public int ModifyPassword(string user_id,string old_password, string new_password)
        {
            IQueryable<User> users = _context.User;
            users = users.Where(u => u.UserId == user_id);

            string old;
            try
            {
                User oldUser = users.First();
                old = oldUser.Password;
                //验证成功
                if(old == old_password)
                {
                    oldUser.Password = new_password;
                    _context.Update(oldUser);
                    _context.SaveChanges();
                    return 0;//成功
                }
                return -2;//旧密码与数据库中的数据不相符
            }
            catch
            {
                return -1;//数据库有问题
            }
        }
    }
}
