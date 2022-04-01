using BudgetManagement.Models;
using BudgetManagement.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManagement.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly IRepositorioCategorias repositorioCategorias;
        private readonly IServiciosUsuarios serviciosUsuarios;

        public CategoriasController(IRepositorioCategorias repositorioCategorias,
            IServiciosUsuarios serviciosUsuarios)
        {
            this.repositorioCategorias = repositorioCategorias;
            this.serviciosUsuarios = serviciosUsuarios;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categorias = await repositorioCategorias.Obtener(usuarioId);
            return View(categorias);
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
        [HttpGet]
        public async Task<IActionResult> Editar(int id) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPorId(id, usuarioId);
            if(categoria is null)
            {
                return RedirectToAction("NoEncontrado, Home");
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Categoria categoriaEditar) 
        {
            if (!ModelState.IsValid) 
            {
                return View(categoriaEditar);
            }
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categoria = repositorioCategorias.ObtenerPorId(categoriaEditar.Id, usuarioId);
            
            if(categoria is null) 
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            categoriaEditar.UsuarioId = usuarioId;
            await repositorioCategorias.Actualizar(categoriaEditar);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Borrar(int id)
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categoria = await repositorioCategorias.ObtenerPorId(id, usuarioId);
            if (categoria is null) 
            {
                return RedirectToAction("NoEncotrado, Home");
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> BorrarCategoria(int id) 
        {
            var usuarioId = serviciosUsuarios.ObtenerUsuarioId();
            var categorias = await repositorioCategorias.ObtenerPorId(id, usuarioId);
            if(categorias is null) 
            {
                return RedirectToAction("NoEncotrado, Home");
            }
            await repositorioCategorias.Borrar(id);
            return RedirectToAction("Index");

        }
    }
}
