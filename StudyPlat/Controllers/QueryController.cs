using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;
using StudyPlat.Message;
using StudyPlat.Models;

namespace StudyPlat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly ModelContext _context;

        public QueryController(ModelContext context)
        {
            _context = context;
        }
        /*
         * 根据question_id来取回question可能会用到的数据
         */
        [HttpGet]
        [Route("1")]
        public IActionResult GetQuestion(string question_id)
        {
            MQuestion mQuestion = new MQuestion(_context);
            MAnswer mAnswer = new MAnswer(_context);
            Question question = mQuestion.GetQuestion(question_id);
            //如果出现-1的id说明出现了错误
            if(question.QuestionId == "-1")
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "没有相应的题目信息,请检查ID是否出错"
                });
            }
            string Qid = question.QuestionId;
            string[] answerIdArray = new string[5];
            answerIdArray = mAnswer.GetAnswerIdArray(Qid);

            return new JsonResult(new QuestionMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "获取题目信息成功"
                },
                data = new QuestionData
                {
                    pic_url = question.PicUrl,
                    answer_id_list = answerIdArray
                }
            });
        }

        /*
         * 根据answer_id来取一个answer
         */
        [HttpGet]
        [Route("2")]
        public IActionResult GetAnswer([FromQuery]string answer_id )
        {
            MAnswer mAnswer = new MAnswer(_context);
            Answer answer = mAnswer.GetAnswer(answer_id);
            //查找失败
            if (answer.AnswerId == "-1")
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "没有相应的答案信息，请检查ID是否出错"
                });
            }
            else
            {
                return new JsonResult(new AnswerMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "根据答案id获取答案信息成功"
                    },
                    data = new AnswerData
                    {
                        answer_content = answer.AnswerContent,
                        answer_id = answer.AnswerId
                    }
                });
            }
        }

        /*
         * 根据book_id来取一本书
         */
        [HttpGet]
        [Route("3")]
        public IActionResult GetBook([FromQuery]string isbn)
        {
            MBook mBook = new MBook(_context);
            Book book = mBook.GetBook(isbn);
            if(book.Isbn == "-1")
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "没有相应的书本信息，请检查Isbn是否出错"
                });
            }
            else
            {
                return new JsonResult(new BookMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "根据书本id获取书本信息成功"
                    },
                    data = new BookData
                    {
                        isbn = book.Isbn,
                        book_name = book.BookName,
                        author = book.Author,
                        publish_time = book.PublishTime,
                        publisher = book.Publisher,
                        comprehension = book.Comprehension,
                        pic_url = book.PicUrl
                    }
                });
            }

        }

        /*
         * 根据course_id来取一个特定的课程
         */
        [HttpGet]
        [Route("4")]
        public IActionResult GetCourse([FromQuery]string course_id)
        {
            MCourse mCourse = new MCourse(_context);
            Course course = mCourse.GetCourse(course_id);
            if(course.CourseId == "-1")
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "没有相应的课程信息，请检查课程ID是否出错"
                });
            }
            else
            {
                return new JsonResult(new CourseMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "根据课程id获取课程信息成功"
                    },
                    data = new CourseData
                    {
                        course_id = course_id,
                        comprehension = course.Comprehension,
                        course_name = course.CourseName
                    }
                });
            }
        }

        /*
         * 通过关键字搜索一个特定的question
         */
        [HttpGet]
        [Route("5")]
        public IActionResult QueryQuestion([FromQuery]string text)
        {
            //查找的结果为所有可能对应的题目的id

            MQuestion mQuestion = new MQuestion(_context);
            List<string> questionList = mQuestion.QueryQuestion(text);

            return new JsonResult(new QueryMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "获取相关题目ID成功"
                },
                data = new QueryData
                {
                    IdList = questionList
                }
            });
        }

        /*
         * 通过关键字搜索一个特定的书
         */
        [HttpGet]
        [Route("6")]
        public IActionResult QueryBook([FromQuery] string text)
        {
            MBook mBook = new MBook(_context);
            List<string> bookList = mBook.QueryBook(text);
            return new JsonResult(new QueryMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "获取相关书籍ID成功"
                },
                data = new QueryData
                {
                    IdList = bookList
                }
            });
        }

        /*
         * 通过关键字搜索特定的课程
         */
        [HttpGet]
        [Route("7")]
        public IActionResult QueryCourse([FromQuery] string text)
        {
            MCourse mCourse = new MCourse(_context);
            List<string> IdList = mCourse.QueryCourse(text);
            return new JsonResult(new QueryMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "获取相关课程ID成功"
                },
                data = new QueryData
                {
                    IdList = IdList
                }
            });
        }
    }
}
