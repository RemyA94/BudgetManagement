namespace BudgetManagement.Models
{
    public class IndiceCuentaViewModel
    {
        public string TipoCuenta { get; set; }
        public IEnumerable<Cuenta> Cuentas { get; set; }
        public decimal Blanace => Cuentas.Sum(x => x.Balance);
    }
}
