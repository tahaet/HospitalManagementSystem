using Hospital.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{

    public class Prescription
    {
        public int Id { get; set; } 

        public string UserId { get; set; }
    
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }  

        public int TreatmentId { get; set; }
    
        [ForeignKey(nameof(TreatmentId))]
        public Treatment Treatment { get; set; }

        public string Medicine { get; set; } 

        public int Times { get; set; } 

        public int Days { get; set; }  

        [MaxLength(20)]
        public string DayTime { get; set; } 

        public int? TestCategoryId { get; set; }

        [ForeignKey(nameof(TestCategoryId))]
        public TestCategory? TestCategory { get; set; }  

        public string? PaymentStatus { get; set; } 

        public string? History { get; set; }
    }

};