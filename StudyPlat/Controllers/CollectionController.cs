using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;
using StudyPlat.Models;
using StudyPlat.Message;

namespace StudyPlat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ModelContext _context;
        public CollectionController(ModelContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("1")]
        public IActionResult GetQuestion([FromQuery] string user_id)
        {
            MQuestion mQuestion = new MQuestion(_context);
            string[] questionIdArray = new string[50];
            questionIdArray = mQuestion.GetQuestionCollection(user_id);
            return new JsonResult(new CollectionMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "返回了所有收藏题目的ID信息"
                },
                data = new CollectionData
                {
                    idArray = questionIdArray
                }
            });
        }

        [HttpGet]
        [Route("2")]
        public IActionResult GetBook([FromQuery] string user_id)
        {
            MBook mBook = new MBook(_context);
            string[] bookIdCollection = new string[50];
            bookIdCollection = mBook.GetBookCollection(user_id);
            return new JsonResult(new CollectionMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "返回了所有收藏书本的isbn码信息"
                },
                data = new CollectionData
                {
                    idArray = bookIdCollection
                }
            });
        }

        [HttpGet]
        [Route("3")]
        public IActionResult GetCourse([FromQuery] string user_id)
        {
            MCourse mCourse = new MCourse(_context);
            string[] courseIdCollection = new string[50];
            courseIdCollection = mCourse.GetCourseCollection(user_id);
            return new JsonResult(new CollectionMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "返回了所有收藏课程的ID信息"
                },
                data = new CollectionData
                {
                    idArray = courseIdCollection
                }
            });
        }

        [HttpPost]
        [Route("1")]
        public IActionResult CollectQuestion(string question_id,string user_id)
        {
            MQuestion mQuestion = new MQuestion(_context);
            int num = mQuestion.CollectQuestion(user_id, question_id);
            if(num == 1)//成功
            {
                return new JsonResult(new Header
                {
                    code = 0,
                    message = "题目收藏成功"
                });
            }
            else if(num == -1)
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "已收藏题目数量达到上限，收藏失败"
                });
            }
            else if(num == 1)
            {
                return new JsonResult(new Header
                {
                    code = 1,
                    message = "已收藏该题目"
                });
            }
            else//num ==-2
            {
                return new JsonResult(new Header
                {
                    code = -2,
                    message = "没有对应的题目，请检查相应题目id"
                });
            }
        }

        [HttpPost]
        [Route("2")]
        public IActionResult CollectBook(string user_id,string isbn)
        {
            MBook mBook = new MBook(_context);
            int num = mBook.CollectBook(user_id, isbn);
            if( num == 1)
            {
                return new JsonResult(new Header
                {
                    code = 1,
                    message = "已收藏该书籍"
                });
            }
            else if(num == 0)
            {
                return new JsonResult(new Header
                {
                    code = 0,
                    message = "书籍收藏成功"
                });
            }
            else if(num == -1)
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "已达到收藏书籍上限，收藏失败"
                });
            }
            else
            {
                return new JsonResult(new Header
                {
                    code = -2,
                    message = "没有对应的书籍，请检查书籍的isbn码"
                });
            }
        }

        [HttpPost]
        [Route("3")]
        public IActionResult CollectCourse(string user_id, string course_id)
        {
            MCourse mCourse = new MCourse(_context);
            int num = mCourse.CollectCourse(user_id, course_id);
            if (num == 1)
            {
                return new JsonResult(new Header
                {
                    code = 1,
                    message = "已收藏该课程"
                });
            }
            else if (num == 0)
            {
                return new JsonResult(new Header
                {
                    code = 0,
                    message = "课程收藏成功"
                });
            }
            else if (num == -1)
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "已达到收藏课程上限，收藏失败"
                });
            }
            else
            {
                return new JsonResult(new Header
                {
                    code = -2,
                    message = "没有对应的课程，请检查课程的id"
                });
            }
        }

        [HttpDelete]
        [Route("1")]

        public IActionResult DeCollectQuestion(string user_id, string question_id)
        {
            MQuestion mQuestion = new MQuestion(_context);
            int num = mQuestion.DeCollectQuestion(user_id, question_id);
            if(num == 0)
            {
                return new JsonResult(new Header
                {
                    code = 0,
                    message = "取消收藏成功"
                });
            }
            else
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "取消收藏失败"
                });
            }
        }

        [HttpDelete]
        [Route("2")]

        public IActionResult DeCollectBook(string user_id,string isbn)
        {
            MBook mBook = new MBook(_context);
            int num = mBook.DecollectBook(user_id, isbn);
            if (num == 0)
            {
                return new JsonResult(new Header
                {
                    code = 0,
                    message = "取消收藏成功"
                });
            }
            else
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "取消收藏失败"
                });
            }
        }

        [HttpDelete]
        [Route("3")]
        public IActionResult DecollectCourse(string user_id, string course_id)
        {
            MCourse mCourse = new MCourse(_context);
            int num = mCourse.DeCollectCourse(user_id, course_id);
            if (num == 0)
            {
                return new JsonResult(new Header
                {
                    code = 0,
                    message = "取消收藏成功"
                });
            }
            else
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "取消收藏失败"
                });
            }
        }
    }
}
