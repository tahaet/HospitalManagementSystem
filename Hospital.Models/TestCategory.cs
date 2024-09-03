using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class TestCategory
    {
        public int Id { get; set; }  

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
