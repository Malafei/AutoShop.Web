using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShop.Domain.Entities
{
    [Table("tblCars")]
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Model { get; set; }
        [Required, StringLength(255)]
        public string Mark { get; set; }
        public int Year { get; set; }
    }
}
