﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;

namespace StudyPlat.Models
{
    public class MCourse:Course
    {
        public readonly ModelContext _context;
        public MCourse(ModelContext context)
        {
            CollectionCourse = new HashSet<CollectionCourse>();
            CourseFromMajor = new HashSet<CourseFromMajor>();
            HasBook = new HashSet<HasBook>();
            QuestionFromCourse = new HashSet<QuestionFromCourse>();
            _context = context;
        }

        public Course GetCourse(string course_id)
        {
            IQueryable<Course> courses = _context.Course;
            courses = courses.Where(u => u.CourseId == course_id);
            int num = courses.Count();
            Course course;
            if(num == 1)
            {
                course = courses.First();
            }
            else//代表查找失败了
            {
                course = new Course
                {
                    CourseId = "-1"
                };
            }
            return course;
        }

        public string FindCourse(string course_name)
        {
            IQueryable<Course> courses = _context.Course;
            courses = courses.Where(u => u.CourseName == course_name);
            if(courses.Count() == 1)
            {
                return courses.First().CourseId;
            }
            else if(courses.Count()>1)
            {
                return "-2";//一个course_name对应了两个id'，请检查数据库
            }
            else
            {
                return "-1";//没有相应的课程
            }
        }
        
        public List<string> QueryCourse(string key)
        {
            IQueryable<Course> courses = _context.Course;
            courses = courses.Where(u => u.CourseName.Contains(key));
            int num = courses.Count();
            List<string> courseIdList = new List<string> { };
            Course[] courseArray = courses.ToArray();
            for( int i = 0; i < num; i++ )
            {
                courseIdList.Add(courseArray[i].CourseId);
            }
            return courseIdList;
        }

        public List<string> QueryCourseCollection(string user_id,string text)
        {
            IQueryable<CollectionCourse> collectionCourses = _context.CollectionCourse;
            IQueryable<Course> courses = _context.Course;
            collectionCourses = collectionCourses.Where(u => u.UserId == user_id);
            int num = collectionCourses.Count();
            List<CollectionCourse> collectionCourses1 = collectionCourses.ToList();
            List<string> courseIDList = new List<string> { };
            List<string> queryIDList = new List<string> { };
            for(int i = 0; i < num; i++)
            {
                courseIDList.Add(collectionCourses1[i].CourseId);
            }
            for (int i = 0; i < num; i++)
            {
                Course course = this.GetCourse(courseIDList[i]);
                if(course.CourseName.Contains(text))
                {
                    queryIDList.Add(course.CourseId);
                }
            }
            return queryIDList;
        }

        
        public List<string> GetCourseByMajor(string major_id)
        {
            IQueryable<CourseFromMajor> courseFromMajors = _context.CourseFromMajor;
            List<string> IDList = new List<string> { };
            //如果是全部课程
            if (major_id == "0")
            {
                IQueryable<Course> courses = _context.Course;
                foreach(var row in courses)
                {
                    IDList.Add(row.CourseId);
                }
                return IDList;
            }
            courseFromMajors = courseFromMajors.Where(u => u.MajorId == major_id);
            foreach(var row in courseFromMajors)
            {
                IDList.Add(row.CourseId);
            }
            return IDList;
        }
        public string[] GetCourseCollection(string user_id)
        {
            IQueryable<CollectionCourse> collectionCourses = _context.CollectionCourse;
            collectionCourses = collectionCourses.Where(u => u.UserId == user_id);
            CollectionCourse[] collectionArray= new CollectionCourse[50];
            string[] idArray = new string[50];
            int count = collectionCourses.Count();
            collectionArray = collectionCourses.ToArray();
            for(int i = 0; i < count; i++ )
            {
                idArray[i] = collectionArray[i].CourseId;
            }
            return idArray;
        }

        public int CheckCourse(string course_id)
        {
            IQueryable<Course> courses = _context.Course;
            courses = courses.Where(u => u.CourseId == course_id);
            int num = courses.Count();
            return num;
        }

        public int CollectCourse(string user_id,string course_id)
        {
            IQueryable<CollectionCourse> courses = _context.CollectionCourse;
            IQueryable<CollectionCourse> isRepeat;
            courses = courses.Where(u => u.UserId == user_id);
            isRepeat = courses.Where(u => u.CourseId == course_id);
            int valid = this.CheckCourse(course_id);
            if(valid == 0)
            {
                return -2; //不存在相应的课程
            }
            int repeat = isRepeat.Count();
            if( repeat > 0)
            {
                return 1; //代表已经收藏过了
            }
            int num = courses.Count();
            if(num <50)
            {
                CollectionCourse collect = new CollectionCourse
                {
                    UserId = user_id,
                    CourseId = course_id,
                    CollectTime = DateTime.Now
                };
                _context.Add(collect);
                _context.SaveChanges();
                return 0;//代表成功收藏了
            }
            else
            {
                return -1;//代表收藏个数满了
            }

        }

        public int DeCollectCourse(string user_id, string course_id)
        {
            IQueryable<CollectionCourse> collectionCourses = _context.CollectionCourse;
            collectionCourses = collectionCourses.Where(u => u.UserId == user_id && u.CourseId == course_id);
            int num = collectionCourses.Count();
            if( num == 1)
            {
                CollectionCourse entity = collectionCourses.First();
                _context.Remove(entity);
                _context.SaveChanges();
                return 0;
            }
            else
            {
                return -1;
            }
        }
        
        public int AddCourse(Course course,string major_name)
        {
            MMajor mMajor = new MMajor(_context);
            string major_id = mMajor.FindMajor(major_name);
            if (major_id == "-1")//说明没有对应的major_name
                return -2;
            try
            {
                _context.Add(course);
                _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
            string course_id = this.FindCourse(course.CourseName);
            IQueryable<CourseFromMajor> courseFromMajors = _context.CourseFromMajor;
            CourseFromMajor relation = new CourseFromMajor
            {
                CourseId = course_id,
                MajorId = major_id
            };
            try
            {
                _context.Add(relation);
                _context.SaveChanges();
                return 0;
            }
            catch
            {
                return -1;
            }
        }

        public List<string> RecommendBook(string course_id)
        {//推荐书籍
            List<string> isbnList = new List<string> { };
            IQueryable<HasBook> hasBooks = _context.HasBook;
            hasBooks = hasBooks.Where(u => u.CourseId == course_id);
            foreach(var row in hasBooks)
            {
                isbnList.Add(row.Isbn);
            }
            return isbnList;
        }
        public List<string> RecommendQuestion(string course_id)
        {//推荐题目
            List<string> questionIDList = new List<string> { };
            IQueryable<QuestionFromCourse> questionFromCourses= _context.QuestionFromCourse;
            questionFromCourses = questionFromCourses.Where(u => u.CourseId == course_id);
            foreach (var row in questionFromCourses)
            {
                questionIDList.Add(row.QuestionId);
            }
            return questionIDList;
        }
        public int DeleteCourse(string course_id)
        {
            IQueryable<HasBook> hasBooks = _context.HasBook;
            IQueryable<Course> courses = _context.Course;
            List<string> BookList = new List<string> { };
            MBook mBook = new MBook(_context);
            hasBooks = hasBooks.Where(u => u.CourseId == course_id);
            foreach(var row in hasBooks)
            {
                BookList.Add(row.Isbn);
            }
            foreach(var isbn in BookList)
            {
                int num = mBook.DeleteBook(isbn);
                if (num != 0)
                    return -2;//在删除相关书籍/问题/答案时出现错误
            }
            courses = courses.Where(u => u.CourseId == course_id);
            Course course = courses.First();
            try
            {
                _context.Remove(course);
                _context.SaveChanges();
                return 0;
            }
            catch
            {
                return -3;// 数据库相关操作出现错误
            }
        }
    }
}