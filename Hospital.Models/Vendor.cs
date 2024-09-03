using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Vendor
    {
        public int Id { get; set; }  

        [MaxLength(100)]
        public string Name { get; set; } 

        public string Address { get; set; }  

        [EmailAddress]
        public string Email { get; set; }
        
        [MaxLength(50)]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
