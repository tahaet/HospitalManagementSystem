using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string About { get; set; }

        public string? Details { get; set; } 

    }
}
