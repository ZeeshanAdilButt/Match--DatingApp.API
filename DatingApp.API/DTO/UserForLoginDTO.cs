using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTO
{
    public class UserForLoginDTO
    {
        [Required]
        public string userName { get; set; }

        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage ="You must specify password between 4 and 8 characters")]
        public string password { get; set; }
    }
}
