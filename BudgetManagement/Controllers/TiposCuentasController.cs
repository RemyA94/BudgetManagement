using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManagement.Controllers
{
    public class TiposCuentasController : Controller
    {
        public IActionResult Crear()
        {
            return View();
        }  
        
        [HttpPost]
        public IActionResult Crear(TipoCuenta tipoCuenta )
        {
            if (!ModelState.IsValid) 
            {
                return View(tipoCuenta);
            }
            return View();
        }
    }
}
