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
    public class FeedbackController : ControllerBase
    {
        private readonly ModelContext _context;
        public FeedbackController(ModelContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 进行一个反馈，表单传参，key:content
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "反馈成功"
        ///     }
        ///     
        /// code对应情况:
        /// 0:反馈成功
        /// -1:数据库相关出现问题，请检查相关代码
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddFeedback()
        {
            MFeedbackInfo mFeedbackInfo = new MFeedbackInfo(_context);
            IFormCollection formParams = HttpContext.Request.Form;
            string content = formParams["content"];
            int num = mFeedbackInfo.AddFeedBack(content);
            string message;
            if(num == 0)
            {
                message = "反馈成功";
            }
            else
            {
                message = "数据库相关出现问题，请检查相关代码";
            }
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }

 

    }
}
