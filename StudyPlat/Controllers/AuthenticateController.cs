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
using StudyPlat.IdentityServer;

namespace StudyPlat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ModelContext _context;
        private readonly IAuthenticateService _authenticate; 

        public AuthenticateController(ModelContext context, IAuthenticateService authenticate)
        {
            _context = context;
            _authenticate = authenticate;
        }

        [HttpGet]
        public string GenerateJWT(User user)
        {
            /*
             * 跟数据库验证是不是合适的
             */

            string token;
            if (_authenticate.IsAuthenticated(user, out token))
            {
                return token;
            }
            return "有问题出错啦";
        }
    }
}
