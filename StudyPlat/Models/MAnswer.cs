﻿using System;
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
            answers = answers.Where(u => u.AnswerContent.Contains(answer_content));
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
                AnswerContent = answer_content,
                Approve = 0
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

        public string GetExpertName(string answer_id)
        {
            MUser mUser = new MUser(_context);
            IQueryable<GiveAnswer> giveAnswers = _context.GiveAnswer;
            giveAnswers = giveAnswers.Where(u => u.AnswerId == answer_id);
            string expert_id = giveAnswers.First().ExpertId;
            User expert = mUser.findUser(expert_id);
            return expert.UserName;
        }

        public int ApproveAnswer(string answer_id,string user_id)
        {
            IQueryable<Answer> answers = _context.Answer;
            answers = answers.Where(u => u.AnswerId == answer_id);
            int num = answers.Count();
            if (num == 0)
                return -1;
            else
            {
                Answer answer = answers.First();
                UserApproveAnswer approve = new UserApproveAnswer
                {
                    AnswerId = answer_id,
                    UserId = user_id
                };
                answer.Approve++;
                try
                {
                    _context.UserApproveAnswer.Add(approve);
                    _context.Answer.Update(answer);
                    _context.SaveChanges();
                    return 0;
                }
                catch
                {
                    return -2;
                }
            }
        }

        public int DisApproveAnswer(string answer_id)
        {
            IQueryable<Answer> answers = _context.Answer;
            answers = answers.Where(u => u.AnswerId == answer_id);
            int num = answers.Count();
            if (num == 0)
                return -1;
            else
            {
                Answer answer = answers.First();
                answer.Approve--;
                try
                {
                    _context.Answer.Update(answer);
                    _context.SaveChanges();
                    return 0;
                }
                catch
                {
                    return -2;
                }
            }
        }
        public int IsApproved(string user_id, string answer_id)
        {
            IQueryable<UserApproveAnswer> userApproveAnswers = _context.UserApproveAnswer;
            userApproveAnswers = userApproveAnswers.Where(u => u.UserId == user_id && u.AnswerId == answer_id);
            int num = userApproveAnswers.Count();
            return num;
        }

        public List<string> GetApproveIDList(string user_id)
        {
            List<string> IDList = new List<string> { };
            IQueryable<UserApproveAnswer> userApproveAnswers = _context.UserApproveAnswer;
            userApproveAnswers = userApproveAnswers.Where(u => u.UserId == user_id);
            foreach(var row in userApproveAnswers)
            {
                IDList.Add(row.AnswerId);
            }
            return IDList;
        }

    }
}
