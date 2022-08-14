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
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly ModelContext _context;

        public PersonalController(ModelContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("1")]
        public IActionResult GetPersonalInformation([FromQuery]string id)//通过user_id来获得个人信息
        {
            MUser mUser = new MUser(_context);

            User user = mUser.findUser(id);

            JsonResult result = new JsonResult(new PersonalMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "个人信息获取成功"
                },
                personalInformation = new PersonalInformation
                {
                    user_name = user.UserName,
                    school = user.SchoolName,
                    major_id = user.MajorId
                }
            });
            return result;

        }

        [HttpGet]
        [Route("2")]
        public IActionResult SetPersonalInformation([FromQuery]string id)//改个人信息
        {
            MUser mUser = new MUser(_context);

            User user = mUser.findUser(id);

            IFormCollection formParameters = HttpContext.Request.Form;
            string name = formParameters["user_name"];
            string school = formParameters["school"];
            string major_id = formParameters["major"];

            user.UserName = name;
            user.SchoolName = school;
            user.MajorId = major_id;

            _context.Update(user);
            _context.SaveChanges();
            JsonResult result = new JsonResult(new PersonalMessage
            {
                header = new Header
                {
                    code = 0,
                    message = "个人信息修改成功"
                },
                personalInformation = new PersonalInformation
                {
                    user_name = user.UserName,
                    school = user.SchoolName,
                    major_id = user.MajorId
                }
            });
            return result;
        }
    }
}
