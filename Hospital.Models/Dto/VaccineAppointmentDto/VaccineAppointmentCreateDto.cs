using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.VaccineAppointmentDto
{
    public class VaccineAppointmentCreateDto
    {
        public int PatientId { get; set; }
        public string NurseId { get; set; }
        public int VaccineId { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeIn { get; set; }
        public bool Taken { get; set; }
        public string DayTime { get; set; }
        public string Room { get; set; }
        public string? Remarks { get; set; }
    }
}
