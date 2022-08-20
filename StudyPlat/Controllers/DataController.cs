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
                int num = mBook.AddBook(newBook);
                string message;
                if (num == 0)
                    message = "保存成功";
                else
                    message = "isbn码出现重复，请检查数据库内是否已经录入相关内容";
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
                int num = mBook.AddBook(newBook);
                string message;
                if (num == 0)
                    message = "保存成功";
                else
                    message = "isbn码出现重复，请检查数据内是否已经录入相关内容";
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
    }
}
