using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Floor
    {
        public int Id { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public int BuildingId { get; set; }

        [ForeignKey(nameof(BuildingId))]
        public Building Building { get; set; }
    }
}
