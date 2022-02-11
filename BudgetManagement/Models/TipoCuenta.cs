using System.ComponentModel.DataAnnotations;

namespace BudgetManagement.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage ="La longitud del campo {0} debe de estar entre {1} y {2}")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }
    }
}
