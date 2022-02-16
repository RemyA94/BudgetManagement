using BudgetManagement.Models;
using Dapper;
using BudgetManagement.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Controllers
{
    public class TiposCuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;

        }
        public IActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TipoCuenta tipoCuenta)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoCuenta);
            }

            tipoCuenta.UsuarioId = 1;
            await repositorioTiposCuentas.Crear(tipoCuenta);

            return View();
        }
    }
}
