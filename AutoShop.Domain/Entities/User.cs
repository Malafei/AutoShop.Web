using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoShop.Domain.Entities
{
    [Table("tblUsers")]
    public class User
    {
        [Key]
        public int Id { get; set; }


        [Required, StringLength(255, MinimumLength = 5)]
        public string Login { get; set; }


        [Required, StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }
        
        
        [Required, StringLength(255)]
        public string KeyWord { get; set; }

    }
}
