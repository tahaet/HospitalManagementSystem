using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models.Dto.MedicineDTo
{
    public class MedicineUpdateDto
    {
        [MaxLength(80)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string GenericName { get; set; }

        [MaxLength(120)]
        public string BatchNo { get; set; }

        [MaxLength(120)]
        public string BarCode { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public double UnitWeight { get; set; }

        public string Type { get; set; }

        public DateTime ManDate { get; set; }

        public DateTime ExpDate { get; set; }

        public decimal Cost { get; set; }

        public decimal RetailCost { get; set; }

        public string Effects { get; set; }

        public int VendorId { get; set; }
    }

}
