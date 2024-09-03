﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class ConsultCategory
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }
        
        [Range(0, double.MaxValue)]
        public double Cost { get; set; }

    }
}
