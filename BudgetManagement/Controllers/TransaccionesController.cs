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
        private readonly IRepositorioCategorias repositorioCategorias;

        public TransaccionesController(IServiciosUsuarios serviciosUsuarios,
            IRepositorioTransacciones repositorioTransacciones, 
            IRepositorioCuentas repositorioCuentas, IRepositorioCategorias repositorioCategorias)
        {
            this.serviciosUsuarios = serviciosUsuarios;
            this.repositorioTransacciones = repositorioTransacciones;
            this.repositorioCuentas = repositorioCuentas;
            this.repositorioCategorias = repositorioCategorias;
        }

        public async Task<IActionResult> Crear()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();

            var modelo = new TransaccionCreacionViewModel();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoOperacionId);

            return View(modelo);
        }

        //Este metodo privado nos va a devolver las cuentas de los usuarios.
        private async Task<IEnumerable<SelectListItem>> ObtenerCuentas(int usuarioId) 
        {
            var cuentas = await repositorioCuentas.Buscar(usuarioId);
            return cuentas.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> ObtenerCategorias(int usuarioId, 
            TipoOperacion tipoOperacion)
        {
            var categorias = await repositorioCategorias.Obtener(usuarioId, tipoOperacion);
            return categorias.Select(x => new SelectListItem(x.Nombre, x.Id.ToString()));
        }


        [HttpPost]
        public async Task<IActionResult> ObtenerCategorias([FromBody] TipoOperacion tipoOperacion) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categorias = await ObtenerCategorias(usuarioId, tipoOperacion);
            return Ok(categorias);
            
        }
    }
}
