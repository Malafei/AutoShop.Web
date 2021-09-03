using AutoMapper;
using AutoShop.Domain;
using AutoShop.Domain.Entities;
using AutoShop.Web.Models;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AutoShop.Web.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly AppEFContext _context;
        private readonly IMapper _mapper;


        public CarController(AppEFContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
            //GenerateAuto();
        }


        private void GenerateAuto()
        {
            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year - 10,
                endDate.Month, endDate.Day);

            var faker = new Faker<Car>("uk")
                .RuleFor(x => x.Mark, f => f.Vehicle.Manufacturer())
                .RuleFor(x => x.Model, f => f.Vehicle.Model())
                .RuleFor(x => x.Year, f => f.Date.Between(startDate, endDate).Year);

            for (int i = 0; i < 1000; i++)
            {
                var car = faker.Generate();
                _context.Cars.Add(car);
                _context.SaveChanges();
            }
        }


        public IActionResult Index(SearchCarIndexModel search, int page = 1)
        {
            int showItems = 15; // кількість даних які відображаються на сторінці
            var query = _context.Cars.AsQueryable(); //переводимо IEnumerable в IQueryable

            if (!string.IsNullOrEmpty(search.searchAttribute)) //перевіряємо чи запит не пустий якщо пустий відображаємо всі дані
            {
                query = _context.Cars.Where(x => x.Mark.ToLower().Contains(search.searchAttribute.ToLower())); // перевіряємо марки
                if (query.Count() == 0)
                {
                    query = _context.Cars.Where(x => x.Model.ToLower().Contains(search.searchAttribute.ToLower())); // перевіряємо моделі
                    if (query.Count() == 0)
                        query = _context.Cars.Where(x => x.Year == Int32.Parse(search.searchAttribute)); //перевіряємо рік
                }
            }

            //кількість записів що є у базі;
            int countItems = query.Count();
            var pageCount = (int)Math.Ceiling(countItems / (double)showItems); //визначаємо кількість сторінок поділивши кількість даних на кількість 
                                                                               //даних для відображення при дробовому числі закругляємо до більшого
            //if (pageCount == 0) pageCount = 1; // якшо кількість сторінок 0 то присвоюємо 1 потрібно для уникнення ексепшенна
            if (pageCount < page && page > 1) // якщо кількість сторінок менша від поточної сторінки і поточна сторінка більше одного
                return RedirectToAction(nameof(Index), new { page = pageCount }); //повертаємо останню сторінку
            
            int skipItems = (page - 1) * showItems; // пропускаємо дані по формулі поточну сторінку множимо на кількість відображаючих даних

            query = query.Skip(skipItems).Take(showItems); // відобразили дані по попереднім розрахунках
            HomeIndexViewModel models = new HomeIndexViewModel(); //створили модель 
            models.Cars = query
                .Select(x => _mapper.Map<CarViewModel>(x))
                .ToList(); 
            models.Page = page;
            models.PageCount = pageCount;
            models.Search = search;
            return View(models);
            // присвоїли нові дані і повернули
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CarCreateViewModel carmodel)
        {
            if (!ModelState.IsValid)
                return View(carmodel);

            string fileName = "";
            if (carmodel.Photo != null)
            {
                var ext = Path.GetExtension(carmodel.Photo.FileName);
                fileName = Path.GetRandomFileName() + ext;
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var filePath = Path.Combine(dir, fileName);
                using(var stream = System.IO.File.Create(filePath)) { carmodel.Photo.CopyTo(stream); }
            }


            Car car = new Car
            {
                PathImages = fileName,
                Mark = carmodel.Mark,
                Model = carmodel.Model,
                Year = carmodel.Year,
            };

            _context.Cars.Add(car);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(CarDeleteViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                Car b = _context.Cars.Find(model.Id);

                if (b.PathImages != null)
                {
                    var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    var filePath = Path.Combine(dir, b.PathImages);
                    System.IO.File.Delete(filePath);
                }

                _context.Cars.Remove(b);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch 
            {
                
                return View();
            }
        }


        [HttpGet]
        public IActionResult EditAvto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditAvto(CarEditViewModel carChange)
        {
            try
            {
                Car NewCar = _context.Cars.Find(carChange.Id);
                if (NewCar != null)
                {
                    if (carChange.MarkNew != null)
                        NewCar.Mark = carChange.MarkNew;
                    if (carChange.ModelNew != null)
                        NewCar.Model = carChange.ModelNew;
                    if (carChange.YearNew != 0)
                        NewCar.Year = carChange.YearNew;
                    if (carChange.Photo != null)
                    {
                        if (NewCar.PathImages != null)
                        {
                            var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                            var FilePath = Path.Combine(directory, NewCar.PathImages);
                            System.IO.File.Delete(FilePath);
                        }

                        var ext = Path.GetExtension(carChange.Photo.FileName);
                        string fileName = Path.GetRandomFileName() + ext;
                        var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                        var filePath = Path.Combine(dir, fileName);
                        using (var stream = System.IO.File.Create(filePath)) { carChange.Photo.CopyTo(stream); }

                        NewCar.PathImages = fileName;
                    }

                    if (NewCar != null) /*Перевірка, чі нормально записались данні*/
                    {
                        _context.Entry(NewCar).State = EntityState.Modified; /*Кажем, що такій об'єкт в базі данних вже є і нам потрібно лише редагувати його*/
                        _context.SaveChanges(); /*Сохраняємо*/
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError("Id", "Такого авто не знайдено");
                return View(carChange);

            }
            catch
            {
                return View(carChange);
            }
        }
    }
}
