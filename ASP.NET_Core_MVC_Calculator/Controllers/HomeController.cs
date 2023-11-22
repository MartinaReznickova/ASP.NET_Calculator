using ASP.NET_Core_MVC_Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ASP.NET_Core_MVC_Calculator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Calculator calculator = new Calculator();
            return View(calculator);
        }

        [HttpPost]
        public IActionResult Index(Calculator calc)
        {
            if (calc.AreTwoNumsNextToEachother())
            {
                ViewBag.Chyba = "Mezera mezi dvěmi čísli, chybí operátor.";
            }

            if (calc.IsLetterInInput())
            {
                ViewBag.Chyba += "Příklad nesmí obsahovat písmena.";
            }

            if (calc.IsUnallowedOperatorAtBeginOrEnd())
            {
                ViewBag.Chyba = "Nepovolený znak na konci nebo na začátku vstupu. ";
            }

            if (!calc.IsDotBetweenNums())
            {
                ViewBag.Chyba += "Desetinná tečka musí být mezi dvěmi čísli. ";
            }
            
            if(calc.IsOperatorDuplicate())
            {
                ViewBag.Chyba += "Vstup obsahuje zdvojené operátory. ";
            }
            
            if(!calc.IsNumOfBracketsEven()) {
                ViewBag.Chyba += "Každá závorka musí být uzavřená. ";
            }
            
            if (ModelState.IsValid)
            {
                calc.GetResult();

            }

            return View(calc);
        }

    }
}
