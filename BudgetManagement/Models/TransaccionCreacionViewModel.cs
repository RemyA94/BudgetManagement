using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    //Para poder mostrarles a los usuarios usus cuentas y sus categorias. 
    public class TransaccionCreacionViewModel: Transaccion
    {
        public IEnumerable<SelectListItem> Cuentas { get; set; }
        public IEnumerable<SelectListItem> Categorias { get; set; }

        [Display(Name ="Tipo de operación")]
        public TipoOperacion TipoOperacionId { get; set; } = TipoOperacion.Ingesos;
    }
}
