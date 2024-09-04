using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.DoctorDetailsDto
{
    public class DoctorUpdateDto : DoctorCreateDto
    {
        public int Id { get; set; }
    }
}
