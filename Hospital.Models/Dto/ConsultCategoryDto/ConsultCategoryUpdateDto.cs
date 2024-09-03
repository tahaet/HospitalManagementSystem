using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.ConsultCategoryDto
{
    public class ConsultCategoryUpdateDto
    {
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public double Cost { get; set; }
    }
}
