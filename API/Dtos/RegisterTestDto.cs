using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class RegisterTestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Range(18, 100, ErrorMessage = "The Age must be between 18 and 100.")]
        public int Age { get; set; }
    }


    
}