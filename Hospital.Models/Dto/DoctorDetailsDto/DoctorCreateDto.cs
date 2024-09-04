using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.DoctorDetailsDto
{
    public class DoctorCreateDto
    {
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public DateTime RegDate { get; set; }
        public string Address { get; set; }
        public string Cell { get; set; }
        public int SpecializationId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string Residence { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Duty { get; set; }
        public string Room { get; set; }
        public decimal Fee { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
        public string Days { get; set; }
        public string Image { get; set; }
    }
}
