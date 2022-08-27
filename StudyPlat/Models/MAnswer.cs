using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;

namespace StudyPlat.Models
{
    public class MAnswer:Answer
    {
        public readonly ModelContext _context;
        public MAnswer(ModelContext context)
        {
            ExplainQuestion = new HashSet<ExplainQuestion>();
            GiveAnswer = new HashSet<GiveAnswer>();
            _context = context;
        }

        public string FindAnswer(string answer_content)
        {
            IQueryable<Answer> answers = _context.Answer;
            answers = answers.Where(u => u.AnswerContent == answer_content);
            int num = answers.Count();
            if (num == 1)
                return answers.First().AnswerId;
            else if (num == 0)
                return "-1";//没找到
            else
                return "-2";//出现了多于1的answer
        }

        public Answer GetAnswer(string answer_id)
        {
            IQueryable<Answer> answers = _context.Answer;
            answers = answers.Where(u => u.AnswerId == answer_id);
            Answer answer;
            int num = answers.Count();
            if(num == 1)
            {
                answer = answers.First();
            }
            else
            {
                answer = new Answer
                {
                    AnswerId = "-1"
                };
            }
            return answer; 
        }

        /*
         * 通过一个question_id来获得对应的answerId的顺序表
         */
        public List<string> GetAnswerIdList(string question_id)
        {
            IQueryable<ExplainQuestion> explainQuestions = _context.ExplainQuestion;
            explainQuestions = explainQuestions.Where(u => u.QuestionId == question_id);
            List<ExplainQuestion> mapArray = new List<ExplainQuestion>{ };
            mapArray = explainQuestions.ToList();
            List<string> answerIdList = new List<string> { };
            int num = explainQuestions.Count();
            for (int i = 0; i < num; i++)
            {
                answerIdList.Add(mapArray[i].AnswerId);
            }
            return answerIdList;
        }

        public string findQuestionIDFromAnswerID(string answer_id)
        {
            IQueryable<ExplainQuestion> explainQuestions = _context.ExplainQuestion;
            explainQuestions = explainQuestions.Where(u => u.AnswerId == answer_id);
            return explainQuestions.First().QuestionId;
        }
        public int AnswerQuestion(string question_id,string answer_content,string expert_id)
        {
            string valid = this.FindAnswer(answer_content);
            if (valid != "-1" && valid != "-2")
                return -2;//说明已经有内容相同的答案
            IQueryable<Question> questions = _context.Question;
            questions = questions.Where(u => u.QuestionId == question_id);
            Question question = questions.First();
            question.Status = true;
            Answer answer = new Answer
            {
                AnswerContent = answer_content
            };
            try
            {
                _context.Add(answer);
                _context.SaveChanges();
            }
            catch
            {
                return -1;//数据库出现问题
            }
            string answer_id = this.FindAnswer(answer_content);
            GiveAnswer giveAnswer = new GiveAnswer
            {
                ExpertId = expert_id,
                AdditionDate = DateTime.Now,
                AnswerId = answer_id
            };
            ExplainQuestion explainQuestion = new ExplainQuestion
            {
                QuestionId = question_id,
                AnswerId = answer_id,
                CreateTime = DateTime.Now,
            };
            try
            {
                _context.Add(giveAnswer);
                _context.Add(explainQuestion);
                _context.Update(question);
                _context.SaveChanges();
                return 0;
            }
            catch
            {
                return -1;//数据库出现问题
            }
        }

        public int DeleteAnswer(string answer_id)
        {
            IQueryable<Answer> answers = _context.Answer;
            answers = answers.Where(u => u.AnswerId == answer_id);
            int num = answers.Count();
            if (num == 0)
                return -1;//不存在相应的答案
            try
            {
                Answer answer = answers.First();
                _context.Remove(answer);
                _context.SaveChanges();
                return 0;
            }
            catch
            {
                return -2;//数据库操作出现错误
            }

        }
    }
}
