using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Vaccine
    {
        public int Id { get; set; }  

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength (20)]
        public string Type { get; set; }

        public string Description { get; set; } 

        public int MedicineId { get; set; }

        [ForeignKey(nameof(MedicineId))]
        public Medicine Medicine { get; set; }

        public string Effects { get; set; }  
    }
}
