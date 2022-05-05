using AutoMapper;
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
        private readonly IMapper mapper;

        public TransaccionesController(IServiciosUsuarios serviciosUsuarios,
            IRepositorioTransacciones repositorioTransacciones,
            IRepositorioCuentas repositorioCuentas,
            IRepositorioCategorias repositorioCategorias, IMapper mapper)
        {
            this.serviciosUsuarios = serviciosUsuarios;
            this.repositorioTransacciones = repositorioTransacciones;
            this.repositorioCuentas = repositorioCuentas;
            this.repositorioCategorias = repositorioCategorias;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Crear()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();

            var modelo = new TransaccionCreacionViewModel();
            modelo.Cuentas = await ObtenerCuentas(usuarioId);
            modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoOperacionId);

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(TransaccionCreacionViewModel modelo)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            if (!ModelState.IsValid)
            {
                modelo.Cuentas = await ObtenerCuentas(usuarioId);
                modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoOperacionId);
                return View(modelo);
            }

            //aqui validamos que la cuenta que el usuario manda a travez del formulario sea una cuenta valida. 
            var cuenta = repositorioCuentas.ObtenerPorId(modelo.CuentasId, usuarioId);
            if (cuenta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            //aqui validamos que la categoria que el usuario manda a travez del formulario sea una cuenta valida. 
            var categoria = repositorioCategorias.ObtenerPorId(modelo.CategoriaId, usuarioId);
            if (categoria is null)
            {
                return RedirectToAction("NoEncotrado", "Home");
            }

            modelo.UsuarioId = usuarioId;

            //si el tipo operacion es un gasto entonces lo guardamos como negativo. 
            if (modelo.TipoOperacionId == TipoOperacion.Gastos)
            {

                modelo.Monto *= -1;
            }
            await repositorioTransacciones.Crear(modelo);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var transaccion = await repositorioTransacciones.ObtenerPorId(id, usuarioId);

            if (transaccion is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            //Aqui usamos AutoMapper para mapear de Transaccion a TransaccionActulizaccionViewModel
            // ya que este va a ser el modelo de nuestra vista para editar la transaccion
            var modelo = mapper.Map<TransaccionActulizacionViewModel>(transaccion);

            //si el tipo de operacion es un ingreso entonces asinamos el monto anterio:
            modelo.MontoAnteriorId = modelo.Monto;

            //si el tipo operacion es un gasto entonces lo guardamos como negativo. 
            if (modelo.TipoOperacionId == TipoOperacion.Gastos)
            {
                modelo.Monto *= -1;
            }

            modelo.CuentaAnteriorId = transaccion.CuentasId;
            modelo.Categorias = await ObtenerCategorias(usuarioId, transaccion.TipoOperacionId);
            modelo.Cuentas = await ObtenerCuentas(usuarioId);

            return View(modelo);

        }

        [HttpPost]
        public async Task<IActionResult> Editar(TransaccionActulizacionViewModel modelo)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();

            if (!ModelState.IsValid)
            {
                modelo.Cuentas = await ObtenerCuentas(usuarioId);
                modelo.Categorias = await ObtenerCategorias(usuarioId, modelo.TipoOperacionId);
                return View(modelo);
            }

            var cuenta = await repositorioCuentas.ObtenerPorId(modelo.CuentasId, usuarioId);
            if (cuenta == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var categoria = await repositorioCategorias.ObtenerPorId(modelo.CategoriaId, usuarioId);
            if (categoria == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            //aqui mapeamos de transaccionActualizacionViewModel a Transaccion 
            var transaccion = mapper.Map<Transaccion>(modelo);

            if (modelo.TipoOperacionId == TipoOperacion.Gastos)
            {
                modelo.Monto *= -1;
            }

            await repositorioTransacciones.Actualizar(transaccion,
                modelo.MontoAnteriorId, modelo.CuentaAnteriorId);

            return RedirectToAction("Index");

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
