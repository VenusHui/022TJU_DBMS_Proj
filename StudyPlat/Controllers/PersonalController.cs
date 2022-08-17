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
        /// <summary>
        /// 根据用户ID来获取用户的个人信息，用于个人信息页面，参数:user_id
        /// </summary>
        /// <remarks>
        /// major_name代表major对应的id，返回信息示例 :
        ///     
        ///     Get/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "个人信息获取成功"
        ///         },
        ///         "personalInformation":
        ///         {
        ///             "user_name" : "name",
        ///             "school" : "TJU",
        ///             "major_name" : "CS"
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取个人信息成功
        /// </remarks>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPersonalInformation(string user_id)//通过user_id来获得个人信息
        {
            MUser mUser = new MUser(_context);

            User user = mUser.findUser(user_id);
            string major_id = user.MajorId;
            MMajor mMajor = new MMajor(_context);
            string major_name = mMajor.GetMajor(major_id).MajorName;
            if (major_name == "-1")
                major_name = "";

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
                    major_name = major_name
                }
            });
            return result;

        }
        /// <summary>
        /// 修改个人信息，参数:user_id,同时使用表单进行个人数据的修改，Key:user_name/school/major_id
        /// 传递的个人数据分别代表更改后的用户名/学校名称/专业id，其中初步设想是major_id这块的值传入需要做成固定形式
        /// 防止出现奇怪的专业名，为后续的操作带来麻烦,例如提供一个用户可选的下拉栏，其中内容为各个专业名，用户可以
        /// 选择相应专业名，然后前端根据选择情况向后端传递对应的major_id，具体实现可以再进行讨论
        /// </summary>
        /// <remarks>
        /// 返回修改后的用户信息，返回信息示例 :
        ///     
        ///     Post/sample
        ///     {
        ///         "header":
        ///         {
        ///             "code" : 0,
        ///             "message" : "个人信息修改成功"
        ///         },
        ///         "personalInformation":
        ///         {
        ///             "user_name" : "name",
        ///             "school" : "TJU",
        ///             "major_name" : "CS"
        ///         }
        ///     }
        ///  code对应的情况:
        ///  0:获取个人信息成功
        /// </remarks>
        /// <param name="user_id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetPersonalInformation(string user_id)//改个人信息
        {
            MUser mUser = new MUser(_context);
            MMajor mMajor = new MMajor(_context);

            User user = mUser.findUser(user_id);

            IFormCollection formParameters = HttpContext.Request.Form;
            string name = formParameters["user_name"];
            string school = formParameters["school"];
            string major_id = formParameters["major_id"];

            string major_name = mMajor.GetMajor(major_id).MajorName;

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
                    major_name = major_name
                }
            });
            return result;
        }
    }
}
