using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class UserDto
    {
        public UserDto(string email,string name, string token, string pictureUrl)
        {
            Email =email;
            Name =name;
            Token=token;
            PictureUrl = pictureUrl;
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public string? PictureUrl { get; set; }
    }


}