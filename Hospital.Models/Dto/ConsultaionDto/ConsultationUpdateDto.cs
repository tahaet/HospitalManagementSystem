using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.ConsultaionDto
{
    public class ConsultationUpdateDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ConsultCategoryId { get; set; }

        public string? Description { get; set; }

        public string? PaymentStatus { get; set; }

        [Required]
        public bool Approved { get; set; }

        [Required]
        public int DoctorId { get; set; }
    }

}
