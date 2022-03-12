using BudgetManagement.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class Cuenta
    {
        public int Id { get; set; }

        [PrimeraLetraMayuscula]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }

        [Display(Name = "Tipo Cuenta")]
        public int TipoCuentasId { get; set; }

        public decimal Balance { get; set; }
        [StringLength(maximumLength: 1000)]

        public string Descripcion { get; set; }
        public string TipoCuenta { get; set; }

    }
}
