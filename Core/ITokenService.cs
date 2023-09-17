using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core
{
    public interface ITokenService
    {
        string CreateToken(AppUser user,string role);
    }
}