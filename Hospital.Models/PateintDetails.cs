using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{

    public class PatientDetails
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

        public string Cell { get; set; }

        public DateTime? BirthDate { get; set; }  

        public string Residence { get; set; }

        public string Email { get; set; }

        public string Guardian { get; set; }

        public string Relation { get; set; }

        [MaxLength(20)]
        public string Gender { get; set; }

        [MaxLength(20)]
        public string? StatusPatient { get; set; }

        [MaxLength(30)]
        public string? PatientType { get; set; }

        public string Image { get; set; }
    }
}
