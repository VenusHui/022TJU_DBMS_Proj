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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly ModelContext _context;

        public QueryController(ModelContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 获取特定题目，根据question_id来取回question可能会用到的数据，参数：question_id
        /// </summary>
        /// <remarks>
        /// answer_id_list是一个最多包含5个答案ID的数组
        /// pic_url是图片的相应地址，可能为空，此时答案不附图片
        /// 返回题目信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取题目信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "pic_url" : "http//:",
        ///             "answer_id_list": [""]
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取题目信息成功
        ///  -1:没有相应题目信息，请检查ID是否出错
        /// </remarks>
        /// <param name="question_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10,VaryByQueryKeys =new string[] {"question_id" })]
        public IActionResult GetQuestion(string question_id)
        {
            
            MQuestion mQuestion = new MQuestion(_context);
            MAnswer mAnswer = new MAnswer(_context);
            Question question = mQuestion.GetQuestion(question_id);
            //如果出现-1的id说明出现了错误
            if(question.QuestionId == "-1")
            {
                return new JsonResult(new QuestionMessage
                {
                    header = new Header
                    {
                        code = -1,
                        message = "没有相应的题目信息,请检查ID是否出错"
                    },
                    data = new QuestionData
                    {
                        pic_url = "",
                        answer_id_list = new string[50]
                    }
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

        /// <summary>
        /// 根据answer_id来获取特定答案，参数:answer_id
        /// </summary>
        /// <remarks>
        /// answer_id_list是一个最多包含5个答案ID的数组
        /// pic_url是图片的相应地址，可能为空，此时答案不附图片
        /// 返回答案信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取答案信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "answer_content" : "这道题目的解答思路是",
        ///             "answer_id": "001"
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:根据答案id获取答案信息成功
        ///  -1:没有相应的答案信息，请检查ID是否出错
        /// </remarks>
        /// <param name="answer_id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration =10 , VaryByQueryKeys = new string[] { "answer_id"})]
        public IActionResult GetAnswer([FromQuery]string answer_id )
        {
            MAnswer mAnswer = new MAnswer(_context);
            Answer answer = mAnswer.GetAnswer(answer_id);
            //查找失败
            if (answer.AnswerId == "-1")
            {
                return new JsonResult(new AnswerMessage
                {
                    header = new Header
                    {
                        code = -1,
                        message = "没有相应的答案信息，请检查ID是否出错"
                    },
                    data = new AnswerData
                    {
                        answer_content = "-1",
                        answer_id = "-1"
                    }
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

        /// <summary>
        /// 根据isbn取一个特定的书本信息，参数:isbn
        /// </summary>
        /// <remarks>
        /// 返回书籍信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "根据书本id获取书本信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "isbn" : "12346",
        ///             "book_name": "高等数学",
        ///             "author" :"作者",
        ///             "publish_time" : "一个时间戳",
        ///             "publisher" : "出版社",
        ///             "comprehension" : "简介",
        ///             "pic_url": "https//:"
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:根据书本id获取书本信息成功
        ///  -1:没有相应的书本信息，请检查Isbn是否出错
        /// </remarks>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 10,VaryByQueryKeys =new string[] { "isbn"})]
        public IActionResult GetBook([FromQuery]string isbn)
        {
            MBook mBook = new MBook(_context);
            Book book = mBook.GetBook(isbn);
            if(book.Isbn == "-1")
            {
                return new JsonResult(new BookMessage
                {
                    header = new Header
                    {
                        code = -1,
                        message = "没有相应的书本信息，请检查Isbn是否出错"
                    },
                    data = new BookData
                    {
                        isbn = "-1",
                        book_name = "-1",
                        author = "-1",
                        publish_time = DateTime.Now,
                        publisher = "-1",
                        comprehension = "-1",
                        pic_url = "-1"
                    }
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

        /// <summary>
        /// 根据course_id来取一个特定的课程，参数:course_id
        /// </summary>
        /// <remarks>
        /// 返回课程信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "根据课程id获取课程信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "course_id" : "12346",
        ///             "comprehension": "有关计算机硬件的一门课",
        ///             "course_name" :"计算机系统结构",
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:根据课程id获取课程信息成功
        ///  -1:没有相应的课程信息，请检查课程ID是否出错
        ///  </remarks>
        /// <param name="course_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCourse([FromQuery]string course_id)
        {
            MCourse mCourse = new MCourse(_context);
            Course course = mCourse.GetCourse(course_id);
            if(course.CourseId == "-1")
            {
                return new JsonResult(new CourseMessage
                {
                    header = new Header
                    {
                        code = -1,
                        message = "没有相应的课程信息，请检查课程ID是否出错"
                    },
                    data = new CourseData
                    {
                        course_id = "-1",
                        comprehension = "-1",
                        course_name = "-1"
                    }
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

        /// <summary>
        /// 通过关键字搜索题目，参数:text
        /// </summary>
        /// <remarks>
        /// IdList是一个装有相关题目ID的链表
        /// 返回搜索题目信息，返回信息示例 :
        /// 
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取相关题目ID成功"
        ///         },
        ///         "data":
        ///         {
        ///             "IdList" : [""],
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取相关题目ID成功
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
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


        /// <summary>
        /// 通过关键字搜索书籍，参数；text
        /// </summary>
        /// <remarks>
        /// IdList是一个装有相关书籍isbn的链表
        /// 返回搜索书籍信息，返回信息示例 :
        /// 
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取相关书籍isbn码成功"
        ///         },
        ///         "data":
        ///         {
        ///             "IdList" : [""],
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取相关书籍isbn码成功
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult QueryBook([FromQuery] string text)
        {
            MBook mBook = new MBook(_context);
            List<string> bookList = mBook.QueryBook(text);
            return new JsonResult(new QueryMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "获取相关书籍isbn码成功"
                },
                data = new QueryData
                {
                    IdList = bookList
                }
            });
        }

        /// <summary>
        /// 通过关键字搜索相关课程，参数:text
        /// </summary>
        /// <remarks>
        /// IdList是一个装有相关课程id的链表
        /// 返回搜索课程信息，返回信息示例 :
        /// 
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取相关课程ID成功"
        ///         },
        ///         "data":
        ///         {
        ///             "IdList" : [""],
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取相关课程ID成功
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        [HttpGet]
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
