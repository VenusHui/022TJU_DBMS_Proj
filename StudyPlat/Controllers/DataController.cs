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
        /// key:isbn/book_name/author/publisher/year/month/day/comprehension/pic_url/course_name
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
        /// -3:已储存了同名书籍
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

            string id = mBook.FindBook(bookName);
            if (id != "-1")
                return new JsonResult(new Header
                {
                    code = -3,
                    message = "已储存了同名书籍"
                });

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
        /// key:question_stem/pic_url/course_name/book_name/major_name
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
        /// -4:说明存在题干相同的题目
        /// </remarks>
        [HttpPost]
        public IActionResult AddQuestion()
        {
            IFormCollection formParams = HttpContext.Request.Form;
            MQuestion mQuestion = new MQuestion(_context);
            string question_stem = formParams["question_stem"];
            string pic_url = formParams["pic_url"];
            string course_name = formParams["course_name"];
            string book_name = formParams["book_name"];
            string major_name = formParams["major_name"];
            Question newQuestion = new Question
            {
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
                message = "数据库有误，请检查代码后重试";
            else if (num == -2)
                message = "输入的书籍名称有误，请检查数据库后再试";
            else if (num == -3)
                message = "输入的课程名称有误，请检查数据库后再试";
            else
                message = "说明存在题干相同的题目";
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
        /// -2:已经有相同内容的答案
        /// -3回答传入的信息有误，可能是传入的question_id或是expert_id为空，或是回答的答案为空
        /// -4:这个用户不是专家
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
                    code = -3,
                    message = "回答传入的信息有误，可能是传入的question_id或是expert_id为空，或是回答的答案为空"
                });
            }
            MUser mUser = new MUser(_context);
            User expert = mUser.findUser(expert_id);
            if(expert.UserType !=2 )
            {
                return new JsonResult(new Header
                {
                    code = -4,
                    message = "这个用户不是专家"
                });
            }
            int num = mAnswer.AnswerQuestion( question_id, answer_content, expert_id);
            string message;
            if( num == 0)
            {
                message = "成功回答问题";
            }
            else if (num == -1)
            {
                message = "数据库出现问题";
            }
            else
            {
                message = "已经有相同内容的答案";
            }
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
        /// <summary>
        /// 添加一门课程，利用表单形式传参，一次只能绑定一个专业，后续需要添加专业要去调用相应api
        /// key:course_name/comprehension/major_name/pic_url
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "课程添加成功"
        ///     }
        ///     
        /// code对应情况:
        /// 0:课程添加成功
        /// -1:数据库有异常
        /// -2:没有对应的major_name
        /// -3:数据库中已存在同名课程
        /// </remarks>
        /// <returns></returns>

        [HttpPost]
        public IActionResult AddCourse()
        {
            IFormCollection formParams = HttpContext.Request.Form;
            MCourse mCourse = new MCourse(_context);
            string course_name = formParams["course_name"];
            string comprehension = formParams["comprehension"];
            string major_name = formParams["major_name"];
            string pic_url = formParams["pic_url"];

            string id = mCourse.FindCourse(course_name);
            if (id != "-1")
                return new JsonResult(new Header
                {
                    code = -3,
                    message = "数据库中已存在同名课程"
                });
            Course course = new Course
            {
                Comprehension = comprehension,
                CourseName = course_name,
                PicUrl = pic_url
            };
            int num = mCourse.AddCourse(course, major_name);
            string message;
            if (num == -2)
                message = "没有对应的major_name";
            else if (num == -1)
                message = "数据库有异常";
            else
                message = "课程添加成功";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
        /// <summary>
        /// 添加专业的api，使用表单传参
        /// key:major_name
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "专业添加成功"
        ///     }
        ///     
        /// code对应情况:
        /// 0:专业添加成功
        /// -1:该专业已存在
        /// -2:数据库出现错误，请检查
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddMajor()
        {
            MMajor mMajor = new MMajor(_context);
            IFormCollection formParams = HttpContext.Request.Form;
            string major_name = formParams["major_name"];
            string major_id = mMajor.FindMajor(major_name);
            if(major_id != "-1")
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "该专业已存在"
                });
            }
            int num = mMajor.AddMajor(major_name);
            string message;
            if(num == 0)
            {
                message = "专业添加成功";
            }
            else
            {
                message = "数据库出现错误，请检查";
            }
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }

        /// <summary>
        /// 删去特定专业的api，参数:major_id
        /// </summary>
        /// <reamrks>
        /// 返回信息示例 :
        /// 
        ///     Delete/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "删除对应专业成功"
        ///     }
        ///     
        /// code对应情况:
        /// 0:删除对应专业成功
        /// -1:不存在相应的专业，请检查后再试
        /// -2:删除专业对应的课程/书本/题目/回答信息出错
        /// -3:数据库操作出现问题，请检查相关代码后再次尝试
        /// </reamrks>
        /// <param name="major_id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteMajor(string major_id)
        {
            MMajor mMajor = new MMajor(_context);
            Major major = mMajor.GetMajor(major_id);
            if(major.MajorId == "-1")//说明不存在对应的major
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "不存在相应的专业，请检查后再试"
                });
            }
            int num = mMajor.DeleteMajor(major_id);
            string message;
            if (num == 0)
                message = "删除对应专业成功";
            else if (num == -2)
                message = "删除专业对应的课程/书本/题目/回答信息出错";
            else
                message = "数据库操作出现问题，请检查相关代码后再次尝试";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }

        /// <summary>
        /// 删除书本的api，参数;isbn
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Delete/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "删除对应书籍成功"
        ///     }
        ///     
        /// code对应情况:
        /// 0:删除对应书籍成功
        /// -1:不存在对应的书籍
        /// -2:数据库出现问题，请检查代码后再次尝试该操作
        /// </remarks>
        /// <param name="isbn"></param>
        /// <returns></returns>
        /// 

        [HttpDelete]
        public IActionResult DeleteBook(string isbn)
        {
            MBook mBook = new MBook(_context);
            Book book = mBook.GetBook(isbn);
            if(book.Isbn == "-1")
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "不存在对应的书籍"
                });
            }
            int num = mBook.DeleteBook(isbn);
            string message;
            if (num == 0)
                message = "成功删除书籍";
            else if (num == -2)
                message = "在删除相关题目/答案时出错";
            else
                message = "数据库相关操作出错";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }

        /// <summary>
        /// 删除答案，参数answer_id
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Delete/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "成功删除对应答案"
        ///     }
        ///     
        /// code对应情况:
        /// 0:成功删除对应答案
        /// -1:不存在相应的答案
        /// -2:数据库操作出现问题，请检查相关代码
        /// </remarks>
        /// <param name="answer_id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteAnswer(string answer_id)
        {
            MAnswer mAnswer = new MAnswer(_context);
            int num = mAnswer.DeleteAnswer(answer_id);
            string message;
            if(num == 0)
            {
                message = "成功删除对应答案";
            }
            else if(num == -1)
            {
                message = "不存在相应的答案";
            }
            else
            {
                message = "数据库操作出现问题，请检查相关代码";
            }

            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
        /// <summary>
        /// 删除问题，参数:question_id
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Delete/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "成功删除对应答案"
        ///     }
        ///     
        /// code对应情况:
        /// 0:成功删除对应问题
        /// -1:不存在对应的问题，请检查参数
        /// -2:在删除问题所对应的答案时出错
        /// -3:数据库操作出现问题，请检查相关代码
        /// </remarks>
        /// <param name="question_id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteQuestion(string question_id)
        {
            MQuestion mQuestion = new MQuestion(_context);
            int num = mQuestion.DeleteQuestion(question_id);
            string message;
            if (num == 0)
                message = "成功删除对应问题";
            else if (num == -1)
                message = "不存在对应的问题，请检查参数";
            else if (num == -2)
                message = "在删除问题所对应的答案时出错";
            else
                message = "数据库操作出现问题，请检查相关代码";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }

        /// <summary>
        /// 删除课程的接口，参数:course_id
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Delete/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "成功删除对应课程"
        ///     }
        ///     
        /// code对应情况:
        /// 0:成功删除相关课程
        /// -1:不存在对应的课程，请检查参数
        /// -2:在删除相关书籍/问题/答案时出现错误
        /// -3:数据库相关操作出现错误
        /// </remarks>
        /// <param name="course_id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeleteCourse(string course_id)
        {
            MCourse mCourse = new MCourse(_context);
            Course course = mCourse.GetCourse(course_id);
            if(course.CourseId == "-1")
            {
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "不存在对应的课程，请检查参数"
                });
            }
            int num = mCourse.DeleteCourse(course_id);
            string message;
            if (num == 0)
                message = "成功删除相关课程";
            else if (num == -2)
                message = "在删除相关书籍/问题/答案时出现错误";
            else
                message = "数据库相关操作出现错误";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
        /// <summary>
        /// 在管理员界面增添专家用户的接口表单传参
        /// key:expert_name/phone_num/password/major_name
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddExpert()
        {
            MUser mUser = new MUser(_context);
            MMajor mMajor = new MMajor(_context);
            IFormCollection formParams = HttpContext.Request.Form;
            string expert_name = formParams["expert_name"];
            string phone_num = formParams["phone_num"];
            string password = formParams["password"];
            string major_name = formParams["major_name"];
            string expert_id = mUser.GenerateId();
            IQueryable<User> users = _context.User;
            int count = users.Where(u => u.PhoneNumber == phone_num).Count();
            string major_id = mMajor.FindMajor(major_name);
            if (expert_name == null || phone_num == null || password == null || major_name == null)
                return new JsonResult(new Header
                {
                    code = -2,
                    message = "专家姓名/电话号码/密码/专业名为空"
                });
            if (count != 0)
                return new JsonResult(new Header
                {
                    code = -1,
                    message = "该电话号已被注册，请更换电话号码后重试"
                });
            if (major_id == "-1")
                return new JsonResult(new Header
                {
                    code = -3,
                    message = "不存在该专业，请检查专业名"
                });

            User expert = new User
            {
                UserId = expert_id,
                UserName = expert_name,
                PhoneNumber = phone_num,
                UserType = 2,
                Password = password,
                MajorId = major_id
            };
            int num = mUser.AddExpert(expert);
            string message;
            if (num == 0)
                message = "添加专家成功";
            else
                message = "数据库相关操作失败，请检查代码及数据库";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
        /// <summary>
        /// 点赞的api，参数:answer_id,user_id
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "点赞成功"
        ///     }
        ///     
        /// code对应的情况:
        /// 1:您已为该答案点过赞
        /// 0:点赞成功
        /// -1:没有对应的答案，请检查answer_id
        /// -2:数据库相关操作有误
        /// </remarks>
        /// <param name="answer_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ApproveAnswer(string answer_id,string user_id)
        {
            MAnswer mAnswer = new MAnswer(_context);
            int isApproved = mAnswer.IsApproved(user_id, answer_id);
            if (isApproved != 0)
                return new JsonResult(new Header
                {
                    code = 1,
                    message = "您已为该答案点过赞"
                });
            int num = mAnswer.ApproveAnswer(answer_id,user_id);
            string message;
            if (num == 0)
                message = "点赞成功";
            else if (num == -1)
                message = "没有对应的答案，请检查answer_id";
            else
                message = "数据库相关操作有误";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
        /// <summary>
        /// 点踩的api，参数:answer_id
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "点踩成功"
        ///     }
        ///     
        /// code对应的情况:
        /// 0:点踩成功
        /// -1:没有对应的答案，请检查answer_id
        /// -2:数据库相关操作有误
        /// </remarks>
        /// <param name="answer_id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DisApproveAnswer(string answer_id)
        {
            MAnswer mAnswer = new MAnswer(_context);
            int num = mAnswer.DisApproveAnswer(answer_id);
            string message;
            if (num == 0)
                message = "点踩成功";
            else if (num == -1)
                message = "没有对应的答案，请检查answer_id";
            else
                message = "数据库相关操作有误";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
    }
}

