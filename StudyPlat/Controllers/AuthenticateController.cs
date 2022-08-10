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

namespace StudyPlat.Controllers
{
    [Route("api/[controller]")]
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

        [Route("1")]//注册用的获取JWT的方式
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
            string Id = users.First().UserId;
            if( num == 0 )
                return new JsonResult(new IdentityMessage
                {
                    code = 0,
                    message = "手机号或密码有误，请查验后重试",
                    data = new data
                    {
                        user_type = 1,
                        token = null
                    }
                });


            User user = users.First();

            string token;
            if (_authenticate.IsAuthenticated(user, out token))
            {
                return new JsonResult(new IdentityMessage
                {
                    code = 0,
                    message = "登陆成功",
                    data = new data
                    {
                        user_type = 1,
                        token = token
                    }
                });
            }
            return new JsonResult(new IdentityMessage
            {
                code = 0,
                message = "登陆失败，请重试",
                data = new data
                {
                    user_type = 1,
                    token = null
                }
            });
        }

        [Route("2")]
        [HttpGet]
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
                    code = 0,
                    message = "该手机号已被注册，请使用一个新的手机号",
                    data = new data
                    {
                        user_type = 1,
                        token = null
                    }
                });
            }
            string Id = userModel.GenerateId();

            User newUser = new User
            {
                UserId = Id,
                UserName = name,
                UserType = true,
                PhoneNumber = phoneNum,
                Password = password
            };
            if (_authenticate.IsAuthenticated(newUser, out token))
            {
                userModel._context.Add(newUser);
                userModel._context.SaveChanges();
                return new JsonResult(new IdentityMessage
                {
                    code = 0,
                    message = "成功注册",
                    data = new data
                    {
                        user_type = 1,
                        token = token
                    }
                });
            }
            return new JsonResult(new IdentityMessage
            {
                code = 0,
                message = "出现错误，无法保存新注册的用户，请再次注册",
                data = new data
                {
                    user_type = 1,
                    token = null
                }
            });


        }
    }
}
