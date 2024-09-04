using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.VaccineDto
{
    public class VaccineUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string Type { get; set; }

        public string Description { get; set; }

        [Required]
        public int MedicineId { get; set; }

        public string Effects { get; set; }
    }

}
