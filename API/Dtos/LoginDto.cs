using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class LoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }

        public bool NativeValid { get;}

        public bool FBValid { get; set; }

        public LoginDto()
        {
            NativeValid = !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
            FBValid = !string.IsNullOrEmpty(Token);
        }
    }


    
}