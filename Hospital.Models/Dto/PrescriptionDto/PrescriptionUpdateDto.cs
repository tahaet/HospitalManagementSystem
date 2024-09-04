using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.PrescriptionDto
{
    public class PrescriptionUpdateDto
    {
        public string? TreatmentId { get; set; }

        public string? Medicine { get; set; }

        public TimeSpan? Time { get; set; }

        public string? Days { get; set; }

        public string? Take { get; set; }

        public string? TestId { get; set; }

        public bool? Paid { get; set; }

        public string? History { get; set; }
    }

}
