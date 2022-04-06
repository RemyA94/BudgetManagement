using BudgetManagement.Models;
using BudgetManagement.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManagement.Controllers
{
    public class TransaccionesController : Controller
    {
        private readonly IServiciosUsuarios serviciosUsuarios;
        private readonly IRepositorioTransacciones repositorioTransacciones;
        private readonly IRepositorioCuentas repositorioCuentas;

        public TransaccionesController(IServiciosUsuarios serviciosUsuarios,
            IRepositorioTransacciones repositorioTransacciones, IRepositorioCuentas repositorioCuentas)
        {
            this.serviciosUsuarios = serviciosUsuarios;
            this.repositorioTransacciones = repositorioTransacciones;
            this.repositorioCuentas = repositorioCuentas;
        }

        public async Task<IActionResult> Crear()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();

            var modelo = new TransaccionCreacionViewModel();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);

            return View(modelo);
        }

        //Este metodo privado nos va a devolver las cuentas de los usuarios.
        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId) 
        {
            var cuentas = await repositorioCuentas.Buscar(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }
    }
}
