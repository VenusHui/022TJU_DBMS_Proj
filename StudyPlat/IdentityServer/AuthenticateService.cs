using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StudyPlat.Entities;
using StudyPlat.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace StudyPlat.IdentityServer
{
    public class AuthenticateService : IAuthenticateService
    {
        public readonly TokenModel _jwtModel;

        public AuthenticateService(IOptions<TokenModel> tokenModel)
        {
            _jwtModel = tokenModel.Value;
        }
        public bool IsAuthenticated(User user,out string token)
        {
            token = String.Empty;

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier,user.UserId)

            };
            //密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtModel.Secret));

            //凭证
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //生成token
            var jwtToken = new JwtSecurityToken(_jwtModel.Issuer, _jwtModel.Audience, claims,
                expires: DateTime.Now.AddMinutes(_jwtModel.AccessExpiration),
                signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
         }
    }
}
