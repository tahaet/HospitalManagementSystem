using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.PrescriptionDto
{
    public class PrescriptionCreateDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string TreatmentId { get; set; }

        [Required]
        public string Medicine { get; set; }

        public TimeSpan Time { get; set; }

        [Required]
        public string Days { get; set; }

        [Required]
        public string Take { get; set; }

        [Required]
        public string TestId { get; set; }

        public bool Paid { get; set; }

        public string? History { get; set; }
    }

}
