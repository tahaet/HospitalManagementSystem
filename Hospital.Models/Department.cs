using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Department
    {
        public int Id { get; set; }  

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Head { get; set; }

        public string Address { get; set; }

        public int FloorId { get; set; }  

        [ForeignKey(nameof(FloorId))]
        public Floor Floor { get; set; }  

        [MaxLength(50)]
        public string Phone { get; set; }
    }
}
