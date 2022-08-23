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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ModelContext _context;
       
        public DataController(ModelContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 用于添加Book的api，以表单形式传参
        /// key:isbn/book_name/author/publisher/year/month/day/comprehension/pic_url
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "保存成功"
        ///     }
        ///     
        /// code对应情况:
        /// 0:保存成功
        /// -1:isbn码出现重复，请检查数据库内是否已经录入相关内容
        /// -2:isbn码/bookName/picUrl未填写，请完善后再次尝试录入书本信息
        /// </remarks>
        [HttpPost]
        public IActionResult AddBook()
        {
            MBook mBook = new MBook(_context);
            IFormCollection formParameters = HttpContext.Request.Form;
            string isbn = formParameters["isbn"];
            string bookName = formParameters["book_name"];
            string author = formParameters["author"];
            string publisher = formParameters["publisher"];
            string strYear = formParameters["year"];
            string strMonth = formParameters["month"];
            string strDay = formParameters["day"];
            string comprehension = formParameters["comprehension"];
            string picUrl = formParameters["pic_url"];
            string course_name = formParameters["course_name"];

            if(strYear!=null && strMonth!=null && strDay!=null && isbn!=null && bookName!=null && picUrl!=null)
            {
                int year = int.Parse(strYear);
                int month = int.Parse(strMonth);
                int day = int.Parse(strDay);
                Book newBook = new Book
                {
                    Isbn = isbn,
                    Author = author,
                    BookName = bookName,
                    Publisher = publisher,
                    PublishTime = new DateTime(year, month, day),
                    PicUrl = picUrl,
                    Comprehension = comprehension
                };
                int num = mBook.AddBook(newBook,course_name);
                string message;
                if (num == 0)
                    message = "保存成功";
                else if (num == -1)
                    message = "isbn码出现重复，请检查数据库内是否已经录入相关内容";
                else
                    message = "course_name的输入出现了问题，请检查后再次尝试";
                return new JsonResult(new Header
                {
                    code = num,
                    message = message
                });
            }
            else if (isbn != null && bookName != null && picUrl != null)
            {
                Book newBook = new Book
                {
                    Isbn = isbn,
                    Author = author,
                    BookName = bookName,
                    Publisher = publisher,
                    PicUrl = picUrl,
                    Comprehension = comprehension
                };
                int num = mBook.AddBook(newBook,course_name);
                string message;
                if (num == 0)
                    message = "保存成功";
                else if(num == -1)
                    message = "isbn码出现重复，请检查数据内是否已经录入相关内容";
                else
                    message = "course_name的输入出现了问题，请检查后再次尝试";
                return new JsonResult(new Header
                {
                    code = num,
                    message = message
                });
            }
            else
            {
                return new JsonResult(new Header
                {
                    code = -2,
                    message = "isbn码/bookName/picUrl未填写，请完善后再次尝试录入书本信息"
                });
            }

        }

        /// <summary>
        /// 用于添加题目的api，用表单来传参
        /// key:question_stem/pic_url/course_name/book_name
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "保存成功"
        ///     }
        ///     
        /// code对应情况:
        /// 0:保存成功
        /// -1:question_id出现重复，请检查数据库后再试
        /// -2:输入的书籍名称有误，请检查数据库后再试
        /// -3:输入的课程名称有误，请检查数据库后再试
        /// </remarks>
        [HttpPost]
        public IActionResult AddQuestion()
        {
            IFormCollection formParams = HttpContext.Request.Form;
            MQuestion mQuestion = new MQuestion(_context);
            string question_id = mQuestion.GenerateId();
            string question_stem = formParams["question_stem"];
            string pic_url = formParams["pic_url"];
            string course_name = formParams["course_name"];
            string book_name = formParams["book_name"];
            string major_name = formParams["major_name"];
            Question newQuestion = new Question
            {
                QuestionId = question_id,
                Status = false,
                PostTime = DateTime.Now,
                PicUrl = pic_url,
                QuestionStem = question_stem
            };
            int num = mQuestion.AddQuestion(newQuestion, book_name, course_name,major_name);
            string message;
            if (num == 0)
                message = "保存成功";
            else if (num == -1)
                message = "question_id出现重复，请检查数据库后再试";
            else if (num == -2)
                message = "输入的书籍名称有误，请检查数据库后再试";
            else
                message = "输入的课程名称有误，请检查数据库后再试";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
        /// <summary>
        /// 专家回答问题的接口，参数有:question_id/expert_id
        /// 通过表单传递回答的答案，表单的key是answer_content
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "成功回答问题"
        ///     }
        ///     
        /// code对应情况:
        /// 0:成功回答问题
        /// -1:回答传入的信息有误，可能是传入的question_id或是expert_id为空，或是回答的答案为空
        /// -2:这个用户不是专家
        /// </remarks>>
        /// <param name="question_id"></param>
        /// <param name="expert_id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddAnswer(string question_id,string expert_id)
        {
            MAnswer mAnswer = new MAnswer(_context);
            IFormCollection formParameters = HttpContext.Request.Form;
            string answer_content = formParameters["answer_content"];
            if(question_id == null || expert_id == null || answer_content == null || answer_content == "")
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "回答传入的信息有误，可能是传入的question_id或是expert_id为空，或是回答的答案为空"
                });
            }
            MUser mUser = new MUser(_context);
            User expert = mUser.findUser(expert_id);
            if(expert.UserType !=2 )
            {
                return new JsonResult(new Header
                {
                    code = -2,
                    message = "这个用户不是专家"
                });
            }
            string answer_id = mAnswer.GenerateID();
            mAnswer.AnswerQuestion(answer_id, question_id, answer_content, expert_id);
            return new JsonResult(new Header
            {
                code = 0,
                message = "成功回答问题"
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /*
        [HttpPost]
        public IActionResult AddCourse()
        {
            IFormCollection formParams = HttpContext.Request.Form;
            MCourse mCourse = new MCourse(_context);
            string course_id = mCourse.GenerateId();
            string course_name = formParams["course_name"];
            string comprehension = formParams["comprehension"];
            string major_name = formParams["major_name"];

        }*/
    }
}
