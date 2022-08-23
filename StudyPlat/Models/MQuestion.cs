using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;
using StudyPlat.Message;

namespace StudyPlat.Models
{
    public class MQuestion : Question
    {
        public ModelContext _context;
        public MQuestion(ModelContext context)
        {
            _context = context;
            CollectionQuestion = new HashSet<CollectionQuestion>();
            ExplainQuestion = new HashSet<ExplainQuestion>();
            QuestionFromCourse = new HashSet<QuestionFromCourse>();
        }

        public string GenerateId()
        {
            IQueryable<Question> questions = _context.Question;
            return (questions.Count() + 1).ToString();
        }
        public Question GetQuestion(string question_id)
        {
            IQueryable<Question> questions = _context.Question;
            questions = questions.Where(u => u.QuestionId == question_id);
            int num = questions.Count();
            Question question;
            if (num == 1)
            {
                question = questions.First();
            }
            else
            {
                question = new Question
                {
                    Status = false,
                    QuestionId = "-1",
                    PostTime = new DateTime(1000, 1, 1)
                };
            }
            return question;
        }
        public List<string> QueryQuestion(string key)
        {
            IQueryable<Question> questions = _context.Question;
            questions = questions.Where(u => u.QuestionStem.Contains(key));
            List<string> questionIdList = new List<string> { };
            int num = questions.Count();
            Question[] questionsArray = questions.ToArray();
            for(int i = 0; i < num; i++ )
            {
                questionIdList.Add(questionsArray[i].QuestionId);
            }


            return questionIdList;
        }

        public List<string> QueryCollectionQuestion(string user_id,string text)
        {
            IQueryable<CollectionQuestion> collectionQuestions = _context.CollectionQuestion;
            IQueryable<Question> questions = _context.Question;
            collectionQuestions = collectionQuestions.Where(u => u.UserId == user_id);
            int num = collectionQuestions.Count();
            List<CollectionQuestion> collectionQuestions1 = collectionQuestions.ToList();
            List<string> questionIDList = new List<string> { };
            List<string> QueryIDList = new List<string> { };
            for(int i = 0; i < num; i++ )
            {
                questionIDList.Add(collectionQuestions1[i].QuestionId);
            }
            for(int i = 0; i < num; i++)
            {
                Question question = this.GetQuestion(questionIDList[i]);
                if(question.QuestionStem.Contains(text))
                {
                    QueryIDList.Add(question.QuestionId);
                }
            }
            return QueryIDList;
        }


        public string[] GetQuestionCollection(string user_id)//为了让前端获得收藏的题目的id
        {
            IQueryable<CollectionQuestion> collection = _context.CollectionQuestion;

            collection = collection.Where(u => u.UserId == user_id);
            CollectionQuestion[] collectionList = new CollectionQuestion[50];
            collectionList = collection.ToArray();
            int questionNum = collection.Count();
            string[] IdArray = new string[50];

            for(int i =0;i< questionNum;i++)
            {
                IdArray[i]=collectionList[i].QuestionId;
            }
            
            return IdArray;
        }

        //检查是否存在相应的问题
        public int CheckQuestion(string question_id)
        {
            IQueryable<Question> questions = _context.Question;
            questions = questions.Where(u => u.QuestionId == question_id);
            int num = questions.Count();
            return num;
        }

        public int CollectQuestion(string user_id, string question_id)
        {
            IQueryable<CollectionQuestion> collectionQuestions = _context.CollectionQuestion;
            //先找出这个用户收集的所有题目
            collectionQuestions = collectionQuestions.Where(u => u.UserId == user_id);
            IQueryable<CollectionQuestion> isRepeat = collectionQuestions.Where(u => u.QuestionId == question_id);
            int valid = this.CheckQuestion(question_id);
            if(valid == 0)
            {
                return -2; //代表不存在相应的问题
            }
            int repeat = isRepeat.Count();
            //说明已经收藏过了
            if(repeat>0)
            {
                return 1;
            }
            int num = collectionQuestions.Count();
            //如果当前收藏数量没有超过上限
            if(num < 50)
            {
                //进行表的相应操作
                CollectionQuestion newcollect = new CollectionQuestion
                {
                    UserId = user_id,
                    QuestionId = question_id,
                    CollectTime = DateTime.Now
                };
                _context.Add(newcollect);
                _context.SaveChanges();
                return 0;//代表成功收藏
            }
            else
            {
                return -1; //代表此时收藏个数已经达到上限，无法继续收藏
            }
        }

        public int DeCollectQuestion(string user_id,string question_id)
        {
            IQueryable<CollectionQuestion> collectionQuestions = _context.CollectionQuestion;
            collectionQuestions = collectionQuestions.Where(u => u.UserId == user_id && u.QuestionId == question_id);
            int num = collectionQuestions.Count();
            if(num == 1)
            {
                CollectionQuestion entity = collectionQuestions.First();
                _context.Remove(entity);
                _context.SaveChanges();
                return 0;//代表删除成功
            }
            else
            {
                return -1;//代表删除失败,没有找到，说明就没收藏
            }
        }
        
        public int AddQuestion(Question question,string book_name,string course_name,string major_name)
        {
            string question_id = question.QuestionId;
            MBook mBook = new MBook(_context);
            MCourse mCourse = new MCourse(_context);
            MMajor mMajor = new MMajor(_context);
            string major_id = mMajor.FindMajor(major_name);
            string isbn = mBook.FindBook(book_name);
            string course_id = mCourse.FindCourse(course_name);
            if(isbn =="-1" || isbn =="-2")
            {
                return -2; //书名有问题
            }
            if(course_id == "-1" || course_id == "-2")
            {
                return -3;//课程名有问题
            }
            QuestionFromBook questionFromBook = new QuestionFromBook
            {
                QuestionId = question.QuestionId,
                Isbn = isbn
            };
            QuestionFromCourse questionFromCourses = new QuestionFromCourse
            {
                QuestionId = question.QuestionId,
                CourseId = course_id
            };
            QuestionFromMajor questionFromMajors = new QuestionFromMajor
            {
                MajorId = major_id,
                QuestionId = question.QuestionId
            };
            int num = this.CheckQuestion(question_id);
            if(num >= 1)
            {
                return -1;//说明已经有这个题目了，相应的question_id
            }
            else
            {
                _context.Add(questionFromBook);
                _context.Add(questionFromCourses);
                _context.Add(questionFromMajors);
                _context.Add(question);
                _context.SaveChanges();
                return 0;
            }
        }

        public bool getStatus(string question_id)
        {
            IQueryable<Question> questions = _context.Question;
            questions = questions.Where(u => u.QuestionId == question_id);
            return questions.First().Status;
        }

    }
}
