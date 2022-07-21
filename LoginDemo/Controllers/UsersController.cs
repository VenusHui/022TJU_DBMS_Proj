﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginDemo.Models;

namespace LoginDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ModelContext _context;

        public UsersController(ModelContext context)
        {
            _context = context;
        }
        [HttpGet]
        public string Login(string name, string password)
        {
            IQueryable<User> users = _context.User;
            users = users.Where(u => u.UserName == name);
            List<User> sutiUsers = users.ToList();
            if (sutiUsers.Count() == 1 && sutiUsers.First().Password == password)
            {
                return "登录成功";
            }
            else
            {
                return "请检查用户名或密码输入是否正确";
            }
        }

        [HttpGet("id")]
        public string Register(string name, string password)
        {
            User newUser = new User(name, password);
            _context.User.Add(newUser);
            return "ok";
        }
    }
}