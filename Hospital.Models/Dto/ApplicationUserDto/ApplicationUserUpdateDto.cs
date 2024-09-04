using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.ApplicationUserDto
{
    public class ApplicationUserUpdateDto
    {
        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string About { get; set; }

        public string? Details { get; set; }

        public string Email { get; set; }

        public string Role {  get; set; }
    }

}
