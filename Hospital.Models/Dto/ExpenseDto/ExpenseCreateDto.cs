using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.ExpenseDto
{
    public class ExpenseCreateDto
    {
        [MaxLength(150)]
        public string Name { get; set; }

        public int DepartmentId { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [MaxLength(20)]
        public string PaymentStatus { get; set; }
    }


}
