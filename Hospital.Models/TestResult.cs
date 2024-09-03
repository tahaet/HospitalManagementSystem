using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Hospital.Models
{
    public class TestResult
    {
        public int Id { get; set; } 

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } 

        public int TestId { get; set; }
        
        [ForeignKey(nameof(TestId))]
        public TestCategory Test { get; set; }  

        public string Result { get; set; }  

        public string? Description { get; set; }

        [MaxLength(50)]
        public string? PaymentStatus { get; set; } 
    }
}
