using Lab_rab_3_Kirichenko.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Lab_rab_3_Kirichenko.Controllers
{
    public class VegetablesController : Controller
    {
        
        private List<Vegetable> GetVegetablesList()
        {
            return new List<Vegetable>
            {
               new Vegetable { Id = 1, Name = "Картофель" },
               new Vegetable { Id = 2, Name = "Морковь" },
             new Vegetable { Id = 3, Name = "Лук" },
              new Vegetable { Id = 4, Name = "Капуста" },
              new Vegetable { Id = 5, Name = "Свекла" },
              new Vegetable { Id = 6, Name = "Кабачок" },
              new Vegetable { Id = 7, Name = "Баклажан" },
              new Vegetable { Id = 8, Name = "Брокколи" },
              new Vegetable { Id = 9, Name = "Томат" }
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult FirstViewMethod()
        {
            
            var veggies = GetVegetablesList().Take(5).ToList();
            return View(veggies);
        }

        public ActionResult SecondViewMethod()
        {

            var sorted = GetVegetablesList().OrderBy(v => v.Id).ToList();
            return View(sorted);
        }

        public ActionResult ThirdViewMethod()
        {

            var groupedVeggies = 
                GetVegetablesList()
              .GroupBy(v => v.Name[0])
               .OrderBy(g => g.Key)
             .ToList() ;

            return View(groupedVeggies);
        }
    }
}