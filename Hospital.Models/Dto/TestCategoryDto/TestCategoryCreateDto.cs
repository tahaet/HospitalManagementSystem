using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.TestCategoryDto
{
    public class TestCategoryCreateDto
    {
        [MaxLength(100)]
        public string TestName { get; set; }

        [Range(0, double.MaxValue)]
        public double MinValue { get; set; }

        [Range(0, double.MaxValue)]
        public double MaxValue { get; set; }

        [Range(0, double.MaxValue)]
        public double Cost { get; set; }

        public string? Description { get; set; }
    }

}
