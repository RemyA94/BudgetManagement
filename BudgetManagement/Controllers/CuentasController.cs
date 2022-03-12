using BudgetManagement.Models;
using BudgetManagement.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Controllers
{
    public class CuentasController : Controller
    {
        private readonly IRepositorioTiposCuentas repositorioTiposCuentas;
        private readonly IServiciosUsuarios serviciosUsuarios;
        private readonly IRepositorioCuentas repositorioCuentas;

        public CuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
            IServiciosUsuarios serviciosUsuarios, IRepositorioCuentas repositorioCuentas)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.serviciosUsuarios = serviciosUsuarios;
            this.repositorioCuentas = repositorioCuentas;
        }
        public async Task<IActionResult> Index() 
        {
            var usarioId = serviciosUsuarios.ObtenerUsuarioId();
            var cuentasConTipoCuenta = await repositorioCuentas.Buscar(usarioId);
        }


        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var modelo = new CuentaCreacionViewModel(); 

            modelo.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CuentaCreacionViewModel cuenta)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await repositorioTiposCuentas.ObtenerPorId(cuenta.TipoCuentasId, usuarioId);

            if (tipoCuentas is null) 
            {
                return RedirectToAction("NO Encontrado", "Home");
            }
            if (!ModelState.IsValid) 
            {
                cuenta.TiposCuentas = await ObtenerTiposCuentas(usuarioId);
                return View(cuenta);
            }
            await repositorioCuentas.Crear(cuenta);
            return RedirectToAction("Index");


        }
        private async Task<IEnumerable<SelectListItem>> ObtenerTiposCuentas(int usuarioId) 
        {
            var tipoCuentas = await repositorioTiposCuentas.Obtener(usuarioId);
            return tipoCuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
