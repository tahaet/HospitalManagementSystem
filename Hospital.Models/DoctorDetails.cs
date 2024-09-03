using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class DoctorDetails
    {
        public int Id { get; set; } 

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }
        
        public string IdNumber { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Address { get; set; }

        public string? Cell { get; set; }

        public int SpecializationId { get; set; }

        [ForeignKey(nameof(SpecializationId))]  
        public Specialization Specialization { get; set; }  

        public int DepartmentId { get; set; }
        
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; } 

        public int DesignationId { get; set; }
        
        [ForeignKey(nameof(DesignationId))]
        public Designation Designation { get; set; }  
        public string Residence { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; } 

        [MaxLength(20)]
        public string Duty { get; set; }

        [MaxLength(50)]
        public string Room { get; set; }

        public decimal Fee { get; set; }

        public TimeSpan? TimeIn { get; set; }  

        public TimeSpan? TimeOut { get; set; }

        [MaxLength(20)]
        public string Days { get; set; } 

        public string Image { get; set; }
    }
}
