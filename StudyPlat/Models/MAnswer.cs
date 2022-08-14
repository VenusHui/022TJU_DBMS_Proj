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
        public string[] GetAnswerIdArray(string question_id)
        {
            IQueryable<ExplainQuestion> explainQuestions = _context.ExplainQuestion;
            explainQuestions = explainQuestions.Where(u => u.QuestionId == question_id);
            ExplainQuestion[] mapArray = new ExplainQuestion[5];
            mapArray = explainQuestions.ToArray();
            string[] answerIdArray = new string[5];
            int num = explainQuestions.Count();
            for (int i = 0; i < num; i++)
            {
                answerIdArray[i] = mapArray[i].AnswerId;
            }
            return answerIdArray;
        }
    }
}
