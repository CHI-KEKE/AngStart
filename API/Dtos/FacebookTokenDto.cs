using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class FacebookTokenDto
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Token_type { get; set; }
    }

    public class UserProfileDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Data Picture { get; set; }
    }

    public class Data
    {
        public int Height { get; set; }
        public bool Is_silhouette { get; set;}
        public string Url { get; set; }
        public int Width { get; set; }
    }
}