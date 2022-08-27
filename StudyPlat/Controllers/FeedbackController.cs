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
        private static object obj = new object();
        private readonly ModelContext _context;
        public FeedbackController(ModelContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 获取反馈信息的api，参数:feedback_id
        /// </summary>
        /// <remarks>
        /// 返回反馈信息，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "获取相应反馈信息成功"
        ///         },
        ///         "data":
        ///         {
        ///             "content" : "http//:",
        ///             "feedback_id": [""]
        ///             "isfinished":"false",
        ///             "time":"2022-8-27"
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取相应反馈信息成功
        ///  -1:没有找到相应反馈信息，请检查参数是否正确
        /// </remarks>
        /// <param name="feedback_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFeedBack(string feedback_id)
        {
            lock(obj)
            {
                MFeedbackInfo mFeedbackInfo = new MFeedbackInfo(_context);
                FeedbackInfo feedbackInfo = mFeedbackInfo.GetFeedBack(feedback_id);
                if (feedbackInfo.FeedbackId == "-1")
                    return new JsonResult(new FeedbackMessage
                    {
                        header = new Header
                        {
                            code = -1,
                            message = "没有找到相应反馈信息，请检查参数是否正确"
                        },
                        data = new FeedbackData
                        {
                            content = feedbackInfo.Content,
                            feedback_id = feedback_id,
                            isfinished = feedbackInfo.IsFinished,
                            time = feedbackInfo.PostTime
                        }
                    });
                return new JsonResult(new FeedbackMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "获取相应反馈信息成功"
                    },
                    data = new FeedbackData
                    {
                        content = feedbackInfo.Content,
                        feedback_id = feedback_id,
                        isfinished = feedbackInfo.IsFinished,
                        time = feedbackInfo.PostTime
                    }
                });
            }
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
        /// <summary>
        /// 用于管理员界面展示，先获得已处理反馈的idlist和未处理反馈的idlist
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Get/Sample
        ///     {
        ///         "header" :
        ///         {
        ///             "code" : 0,
        ///             "message" : "成功获得对应的idlist"
        ///         } ,
        ///         "data":
        ///         {
        ///             "finishedList" : ["1"],
        ///             "unfinishedList" :["2"]
        ///         }
        ///     }
        ///     
        /// code对应情况:
        /// 0:成功获得对应的idlist
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFeedbackList()
        {
            lock(obj)
            {
                List<string> unfinishedList = new List<string> { };
                List<string> finishedList = new List<string> { };
                MFeedbackInfo mFeedback = new MFeedbackInfo(_context);
                unfinishedList = mFeedback.FindUnfinished();
                finishedList = mFeedback.FindFinished();
                return new JsonResult(new GetFeedbackMessage
                {
                    header = new Header
                    {
                        code = 0,
                        message = "成功获得对应的idlist"
                    },
                    data = new GetFeedbackData
                    {
                        finishedList = finishedList,
                        unfinishedList = unfinishedList
                    }
                });
            }
        }

        /// <summary>
        /// 修改特定反馈信息的状态，当这条反馈信息为未处理时，会将其转为已处理；当这条反馈信息为已处理时，会将其转为未处理。
        /// 参数:feedback_id
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        /// 
        ///     Post/Sample
        ///     {
        ///         "code" : 0,
        ///         "message" : "反馈信息的状态已被成功改变"
        ///     }
        ///     
        /// code对应情况:
        /// 0:反馈信息的状态已被成功改变
        /// -1:数据库操作出现问题，请检查代码后重试
        /// -2:没找到对应的反馈信息，请检查传入的参数是否正确
        /// </remarks>
        /// <param name="feedback_id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SwitchStatus(string feedback_id)
        {
            MFeedbackInfo mFeedback = new MFeedbackInfo(_context);
            int num = mFeedback.SwitchStatus(feedback_id);
            string message;
            if (num == 0)
                message = "反馈信息的状态已被成功改变";
            else if (num == -1)
                message = "数据库操作出现问题，请检查代码后重试";
            else
                message = "没找到对应的反馈信息，请检查传入的参数是否正确";
            return new JsonResult(new Header
            {
                code = num,
                message = message
            });
        }
 

    }
}
