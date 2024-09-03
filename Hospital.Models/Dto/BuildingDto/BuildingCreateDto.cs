using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.BuildingDto
{
    public class BuildingCreateDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Code { get; set; }

        public string? Description { get; set; }
    }
}
