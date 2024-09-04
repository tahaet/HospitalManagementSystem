using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.PatientDetailsDto
{
    public class PatientDetailsCreateDto
    {
        [Required]
        public string UserId { get; set; }

        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }

        [Required]
        public string IdNumber { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Address { get; set; }

        [Required]
        public string Cell { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        public string Residence { get; set; }

        [Required]
        public string Email { get; set; }

        public string Guardian { get; set; }

        public string Relation { get; set; }

        [MaxLength(20)]
        [Required]
        public string Gender { get; set; }

        [MaxLength(20)]
        public string? StatusPatient { get; set; }

        [MaxLength(30)]
        public string? PatientType { get; set; }

        public string Image { get; set; }
    }

}
