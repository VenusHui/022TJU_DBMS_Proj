using System;
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

        public string GenerateId()
        {
            IQueryable<Course> courses = _context.Course;
            return (courses.Count() + 1).ToString();
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
        /*
        public List<string> GetCourseByMajor(string major_name)
        {
            IQueryable<Major> majors = _context.Major;
            majors = majors.Where(u => u.MajorName == major_name);
            string majorId = majors.First().MajorId;
            IQueryable<Course> courses = _context.Course;
            courses = courses.Where(u => u.);
            int num = courses.Count();
            List<string> courseIdList = new List<string> { };
            Course[] courseArray = courses.ToArray();
            for (int i = 0; i < num; i++)
            {
                courseIdList.Add(courseArray[i].CourseId);
            }
            return courseIdList;
        }*/
        public string[] GetCourseCollection(string user_id)
        {
            IQueryable<CollectionCourse> collectionCourses = _context.CollectionCourse;
            collectionCourses = collectionCourses.Where(u => u.UserId == user_id);
            CollectionCourse[] collectionArray= new CollectionCourse[50];
            string[] idArray = new string[50];
            int count = collectionCourses.Count();
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
        /*
        public int AddCourse(Course course,string major_name)
        {
            MMajor mMajor = new MMajor(_context);
            string major_id = mMajor.FindMajor(major_name);
        }*/
    }
}
