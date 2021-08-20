using AutoMapper;
using AutoShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoShop.Web.Models
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarViewModel>();
        }

    }
}
