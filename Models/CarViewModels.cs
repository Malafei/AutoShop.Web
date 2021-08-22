using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoShop.Web.Models
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        public string PathImage { get; set; }
    }

    public class CarCreateViewModel
    {
        [Required]
        [Display(Name ="Марка")]
        public string Mark { get; set; }
        [Required]
        [Display(Name = "Модель")]
        public string Model { get; set; }
        [Required]
        [Display(Name = "Рік випуску")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Фото")]
        public IFormFile Photo { get; set; }
    }

    public class CarDeleteViewModel
    {
        [Display(Name = "Id автомобіля")]
        public int Id { get; set; }
    }



    public class CarEditViewModel
    {
        [Required]
        [Display(Name = "ID машини в списку")]
        public int Id { get; set; }



        [Display(Name = "Нова Марка")]
        public string MarkNew { get; set; }

        [Display(Name = "Нова Модель")]
        public string ModelNew { get; set; }

        [Display(Name = "Новий Рік випуску для машини")]
        public int YearNew { get; set; }

        [Display(Name = "Фото")]
        public IFormFile Photo { get; set; }
    }


    public class SearchCarIndexModel
    {
        public string searchAttribute { get; set; }
    }
}
