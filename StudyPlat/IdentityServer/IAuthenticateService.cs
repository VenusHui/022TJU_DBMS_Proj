using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyPlat.Entities;

namespace StudyPlat.IdentityServer
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(User user,out string token);
    }
}
