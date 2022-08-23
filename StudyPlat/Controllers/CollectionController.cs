using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;
using StudyPlat.Models;
using StudyPlat.Message;
using Swashbuckle.AspNetCore.Swagger;

namespace StudyPlat.Controllers
{
    /// <summary>
    /// 这是用于收藏使用的类
    /// </summary>

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly ModelContext _context;
        public CollectionController(ModelContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 收藏问题,参数:question_id, user_id
        /// </summary>
        /// <remarks>
        /// header中会返回收藏成功或失败的信息，以收藏成功为例 :
        ///     
        ///     Post/sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "题目收藏成功"
        ///     }
        ///  code对应的情况:
        ///  0:题目收藏成功
        ///  1:已收藏该题目
        ///  -1:已收藏题目数量达到上限，收藏失败
        ///  -2:没有对应的题目，请检查相应题目id
        /// </remarks>
        /// <param name="question_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CollectQuestion(string question_id,string user_id)
        {
            MQuestion mQuestion = new MQuestion(_context);
            int num = mQuestion.CollectQuestion(user_id, question_id);
            if(num == 0)//成功
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

        /// <summary>
        /// 收藏书本，参数: user_id,isbn
        /// </summary>
        /// <remarks>
        /// header中会返回收藏成功或失败的信息，以收藏成功为例 :
        ///       
        ///     Post/sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "书本收藏成功"
        ///     }
        ///  code对应的情况:
        ///  0:书本收藏成功
        ///  1:已收藏该书本
        ///  -1:已收藏书本数量达到上限，收藏失败
        ///  -2:没有对应的书本，请检查isbn
        /// </remarks>
        /// <param name="user_id"></param>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpPost]
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
        /// <summary>
        /// 收藏课程，参数:user_id,course_id
        /// </summary>
        /// <remarks>
        /// header中会返回收藏成功或失败的信息，以收藏成功为例 :
        /// 
        ///     Post/sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "课程收藏成功"
        ///     }
        ///  code对应的情况:
        ///  0:课程收藏成功
        ///  1:已收藏该课程
        ///  -1:已收藏课程数量达到上限，收藏失败
        ///  -2:没有对应的课程，请检查课程ID
        /// </remarks>
        /// <param name="user_id"></param>
        /// <param name="course_id"></param>
        /// <returns></returns>
        [HttpPost]
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
        /// <summary>
        /// 问题取消收藏,参数:user_id,question_id
        /// </summary>
        /// <remarks>
        /// header中会返回行动是否执行成功的信息，以取消收藏成功为例 :
        ///     
        ///     Delete/sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "取消收藏成功"
        ///     }
        ///  code对应的情况:
        ///  0:取消收藏成功
        ///  -1:取消收藏失败
        /// </remarks>
        /// <param name="user_id"></param>
        /// <param name="question_id"></param>
        /// <returns></returns>
        [HttpDelete]
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
        /// <summary>
        /// 书本取消收藏，参数:user_id,isbn
        /// </summary>
        /// <remarks>
        /// header中会返回行动是否执行成功的信息，以取消收藏成功为例 :
        ///     
        ///     Delete/sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "取消收藏成功"
        ///     }
        ///  code对应的情况:
        ///  0:取消收藏成功
        ///  -1:取消收藏失败
        /// </remarks>
        /// <param name="user_id"></param>
        /// <param name="isbn"></param>
        /// <returns></returns>
        [HttpDelete]
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
        /// <summary>
        /// 课程取消收藏,参数:user_id,course_id
        /// </summary>
        /// <remarks>
        /// header中会返回行动是否执行成功的信息，以取消收藏成功为例 :
        ///     
        ///     Delete/sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "取消收藏成功"
        ///     }
        ///  code对应的情况:
        ///  0:取消收藏成功
        ///  -1:取消收藏失败
        /// </remarks>
        /// <param name="user_id"></param>
        /// <param name="course_id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeCollectCourse(string user_id, string course_id)
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
