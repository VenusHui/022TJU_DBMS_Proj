﻿using Microsoft.AspNetCore.Http;
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
        
    }
}