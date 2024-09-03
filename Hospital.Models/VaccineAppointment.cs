using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class VaccineAppointment
    {
        public int Id { get; set; }  

        public string PatientId { get; set; }
        
        [ForeignKey(nameof(PatientId))]
        public PatientDetails Patient { get; set; }  
        public string NurseId { get; set; }

        [ForeignKey(nameof(NurseId))]
        public ApplicationUser Nurse { get; set; }  

        public int VaccineId { get; set; }

        [ForeignKey(nameof(VaccineId))]
        public Vaccine Vaccine { get; set; } 


        public DateTime Date { get; set; } 

        public DateTime TimeIn { get; set; }

        public bool Taken { get; set; }

        public string DayTime { get; set; } 


        [MaxLength(50)]
        public string Room { get; set; }  

        public string? Remarks { get; set; } 
    }
}
