using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;
using StudyPlat.Models;
using StudyPlat.IdentityServer;
using StudyPlat.Message;

namespace StudyPlat.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ModelContext _context;
        private readonly IAuthenticateService _authenticate; 

        public AuthenticateController(ModelContext context,IAuthenticateService authenticate)
        {
            _context = context;
            _authenticate = authenticate;
        }
        /// <summary>
        /// 登陆用的获取JWT的方式，传参通过表单来传，他们的key分别是:user_name/password/phone_number
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "登陆成功"
        ///         },
        ///         "data":
        ///         {
        ///             "user_type" : 1,
        ///             "token":""
        ///         }
        ///     }
        /// 其中，user_type: 1代表用户，2代表专家，3代表管理员
        /// code对应的情况:
        /// 0:登陆成功
        /// -1:手机号或密码有误
        /// -2：其他原因导致的登陆失败
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LoginGenerateJWT()
        {
            IFormCollection formParameter = HttpContext.Request.Form;
            string name = formParameter["user_name"];
            string password = formParameter["password"];
            string phone = formParameter["phone_number"];
            /*
             * 跟数据库验证是不是合适的
             */
            IQueryable<User> users = _context.User;
            users = users.Where(u => (u.PhoneNumber == phone) && (u.Password == password));
            int num = users.Count();
            if( num == 0 )
                return new JsonResult(new IdentityMessage
                {
                    data = new IdentityData
                    {
                        user_type = 1,
                        token = null
                    },
                    header = new Header
                    {
                        code = -1,
                        message = "手机号或密码有误，请查验后重试"
                    }
                });


            User user = users.First();

            string token;
            if (_authenticate.IsAuthenticated(user, out token))
            {
                return new JsonResult(new IdentityMessage
                {
                    data = new IdentityData
                    {
                        user_type = user.UserType,
                        token = token
                    },
                    header = new Header
                    {
                        code = 0,
                        message = "登陆成功"
                    }
                });
            }
            return new JsonResult(new IdentityMessage
            {
                data = new IdentityData
                {
                    user_type = 1,
                    token = null
                },
                header = new Header
                {
                    code = -2,
                    message = "登陆失败，请重试"
                }
            });
        }

        /// <summary>
        /// 注册，注册成功后返回JWT，当前传参方式使用表单，key: user_name/password/phone_number
        /// </summary>
        /// <remarks>
        /// 返回信息示例 :
        ///     
        ///     Post/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "注册成功"
        ///         },
        ///         "data":
        ///         {
        ///             "user_type" : 1,
        ///             "token":""
        ///         }
        ///     }
        /// 其中，user_type: 1代表用户，2代表专家，3代表管理员
        /// code对应的情况:
        /// 0:登陆成功
        /// -1:该手机号已经被注册
        /// -2：其他原因导致的注册失败
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegisterGenerateJWT()
        {
            IFormCollection formParameters = HttpContext.Request.Form;
            string name = formParameters["user_name"];
            string password = formParameters["password"];
            string phoneNum = formParameters["phone_number"];
            string token;

            MUser userModel = new MUser(_context);

            IQueryable < User > users= _context.User;
            //查找有没有重复项

            int count = users.Where(u => u.PhoneNumber == phoneNum).Count();
            if(count !=0)//有重复，不能注册
            {
                //return "该手机号已被注册，请使用一个新的手机号";
                return new JsonResult(new IdentityMessage
                {
                    data = new IdentityData
                    {
                        user_type = 1,
                        token = null
                    },
                    header = new Header
                    {
                        code = -1,
                        message = "该手机号已被注册，请使用一个新的手机号",
                    }
                });
            }
            string Id = userModel.GenerateId();

            User newUser = new User
            {
                UserId = Id,
                UserName = name,
                UserType = 1,
                PhoneNumber = phoneNum,
                Password = password
            };
            if (_authenticate.IsAuthenticated(newUser, out token))
            {
                userModel._context.Add(newUser);
                userModel._context.SaveChanges();
                return new JsonResult(new IdentityMessage
                {
                    data = new IdentityData
                    {
                        user_type = 1,
                        token = token
                    },
                    header = new Header
                    {
                        code = 0,
                        message = "成功注册",
                    }
                });
            }
            return new JsonResult(new IdentityMessage
            {
                data = new IdentityData
                {
                    user_type = 1,
                    token = null
                },
                header = new Header
                {
                    code = -2,
                    message = "出现错误，无法保存新注册的用户，请再次注册",
                }
            });


        }

        
    }
}
