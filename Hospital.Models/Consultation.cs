using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Consultation
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public PatientDetails Patient { get; set; }  

        public int ConsultCategoryId { get; set; }

       
        [ForeignKey(nameof(ConsultCategoryId))]
        public ConsultCategory ConsultCategory { get; set; }  

        public string? Description { get; set; }  

        public string? PaymentStatus { get; set; }

        public bool Approved { get; set; } 

        public int DoctorId { get; set; }
        
        [ForeignKey(nameof(DoctorId))]
        public DoctorDetails Doctor { get; set; }  
    }
}
