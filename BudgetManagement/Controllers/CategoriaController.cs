using BudgetManagement.Models;
using BudgetManagement.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManagement.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IServiciosUsuarios serviciosUsuarios;

        public CategoriaController(IRepositorioCategorias repositorioCategorias, 
            IServiciosUsuarios serviciosUsuarios)
        {
            this.repositorioCategorias = repositorioCategorias;
            this.serviciosUsuarios = serviciosUsuarios;
        }

        [HttpGet]
        public IActionResult Crear() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Categoria categoria) 
        {

           
            var usarioId = serviciosUsuarios.ObtenerUsuarioId();
            categoria.UsuarioId = usarioId;
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }
            await repositorioCategorias.Crear(categoria);
            return RedirectToAction("Index");


        }

    }
}
