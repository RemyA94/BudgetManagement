namespace BudgetManagement.Models
{
    public class TransaccionActulizacionViewModel: TransaccionCreacionViewModel
    {
        public int CuentaAnteriorId { get; set; }
        public decimal MontoAnteriorId { get; set; }
    }
}
