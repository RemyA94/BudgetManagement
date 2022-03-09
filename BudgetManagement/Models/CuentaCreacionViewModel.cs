using BudgetManagement.Validaciones;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class CuentaCreacionViewModel :Cuenta
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }

    }
}
