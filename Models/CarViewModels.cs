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
    }

    public class CarCreateViewModel
    {
        [Display(Name ="Марка")]
        public string Mark { get; set; }
        [Display(Name = "Модель")]
        public string Model { get; set; }
        [Display(Name = "Рік випуску")]
        public int Year { get; set; }
    }

    public class CarDeleteViewModel
    {
        [Display(Name = "Id автомобіля")]
        public int Id { get; set; }
    }



    public class CarEditViewModel
    {
        [Display(Name = "ID машини в списку")]
        public int Id { get; set; }



        [Display(Name = "Нова Марка")]
        public string MarkNew { get; set; }
        [Display(Name = "Нова Модель")]
        public string ModelNew { get; set; }
        [Display(Name = "Новий Рік випуску для машини")]
        public int YearNew { get; set; }
    }


    public class SearchCarIndexModel
    {
        public string searchAttribute { get; set; }
    }
}
