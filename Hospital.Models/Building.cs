using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class Building
    {
        public int Id { get; set; }  

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Code { get; set; }

        public string? Description { get; set; }
    }

}
