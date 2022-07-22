using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginDemo.Models;

namespace LoginDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ModelContext _context;

        public UsersController(ModelContext context)
        {
            _context = context;
        }
        [HttpPost]
        public JsonResult Login(string name, string password)
        {
            IQueryable<User> users = _context.User;
            users = users.Where(u => u.UserName == name);
            List<User> sutiUsers = users.ToList();
            if (sutiUsers.Count() == 1 && sutiUsers.First().Password == password)
            {
                return new JsonResult(new Message
                {
                    Status = Message.STATUS_SUCCESS,
                    Reply = "登录成功",
                });
            }
            else
            {
                return  new JsonResult(new Message
                {
                    Status = Message.STATUS_ERROR,
                    Reply = "请检查用户名或密码输入是否正确"
                }); ;
            }
        }

        [HttpGet]
        public JsonResult Register(string name, string password)
        {
            IQueryable<User> users = _context.User;
            users = users.Where(u => u.UserName == name);
            List<User> sutiUsers = users.ToList();
            if (sutiUsers.Count() > 0)
            {
                return new JsonResult(new Message
                {
                    Status = Message.STATUS_ERROR,
                    Reply = "用户名已被注册，请选择一个新的用户名",
                });
            }
            User newUser = new User { UserName = name, Password = password };
            _context.User.Add(newUser);
            _context.SaveChanges();
            return new JsonResult(new Message
            {
                Status = Message.STATUS_SUCCESS,
                Reply = "注册成功",
            });
        }

    }
}
