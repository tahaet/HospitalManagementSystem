using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.Department
{
    public class DepartmentUpdateDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Head { get; set; }

        public string Address { get; set; }

        public int FloorId { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }
    }
}
