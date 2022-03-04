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
        private readonly IServiciosUsuarios serviciosUsuarios;

        public TiposCuentasController(IRepositorioTiposCuentas repositorioTiposCuentas,
            IServiciosUsuarios serviciosUsuarios)
        {
            this.repositorioTiposCuentas = repositorioTiposCuentas;
            this.serviciosUsuarios = serviciosUsuarios;
        }

        public async Task<IActionResult> Index() 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId(); 
            var tipoCuentas= await repositorioTiposCuentas.Obtener(usuarioId);
            return View(tipoCuentas);
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

            tipoCuenta.UsuarioId = serviciosUsuarios.ObtenerUsuarioId();

            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(tipoCuenta.Nombre, tipoCuenta.UsuarioId);
            if (yaExisteTipoCuenta) 
            {
                ModelState.AddModelError(nameof(tipoCuenta.Nombre),
                      $"El nombre {tipoCuenta.Nombre} ya existe.");

                return View(tipoCuenta);
            }
            await repositorioTiposCuentas.Crear(tipoCuenta);           
            return RedirectToAction("Index");
        }

        // Este metodo nos permite cargar el registro por el Id del usuario.
        [HttpGet]
        public async Task<IActionResult> Editar(int Id) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await repositorioTiposCuentas.ObtenerPorId(Id , usuarioId);
            if (tipoCuentas is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuentas);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(TipoCuenta tipoCuenta) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tipoCuentaExiste = await repositorioTiposCuentas.ObtenerPorId(tipoCuenta.Id, usuarioId);
            if(tipoCuentaExiste is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioTiposCuentas.Actualizar(tipoCuenta);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);
            if(tipoCuentas is null) //O no existe o el usuario no tiene los permisos para visualizarlos. 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoCuentas);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarTipoCuentas(int id) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var tipoCuentas = await repositorioTiposCuentas.ObtenerPorId(id, usuarioId);
            if (tipoCuentas is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await repositorioTiposCuentas.Borrar(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> VerificarExisteTipoCuenta(string nombre) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var yaExisteTipoCuenta = await repositorioTiposCuentas.Existe(nombre, usuarioId);
            if (yaExisteTipoCuenta) 
            {
                return Json($"El nombre {nombre} ya existe");
            }
            return Json(true);
        }

    }
}
