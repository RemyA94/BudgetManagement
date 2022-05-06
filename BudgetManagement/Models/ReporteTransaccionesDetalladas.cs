namespace BudgetManagement.Models
{
    public class ReporteTransaccionesDetalladas
    {
        //aqui le mostramos las transacciones al usuario de tal a tal fecha. 
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }


        //aqui tomando como parametro una fecha inico y una fecha fin vamos a calcular el total de depositos [ingresos] y retiros[gastos]
        public IEnumerable<TransaccionesPorFecha> TransaccionesAgrupadas { get; set; }
        public decimal BalanceDepositos => TransaccionesAgrupadas.Sum(x => BalanceDepositos);
        public decimal BalanceRetiros => TransaccionesAgrupadas.Sum(x => BalanceRetiros);
        //calculamso el total que vienen siendo la resta entre depositos y retiros
        public decimal Total => BalanceDepositos - BalanceRetiros;



        //aqui tenemos las transacciones agrupadas por fecha, asi le decimos cuantas transacciones 
        //hizo en una fecha determinada
        public class TransaccionesPorFecha
        {
            //Esta FechaTransaccion nos dice a que fecha corresponde las transacciones ubicadas aqui
            public DateTime FechaTransaccion { get; set; }

            //aqui colocamos las transacciones
            public IEnumerable<Transaccion> Transacciones { get; set; }

            //una vez obtenidas las transacciones buscamos el balance de los depositos y el balance de los 
            //retiros. Ya que es una informacion que el usuario necesita saber. Asi el sabe cuanto deposito hoy y cuanto retiro.

            //buscamos el balance de los depositos
            public decimal BalanceDepositos => Transacciones
                .Where(x => x.TipoOperacionId == TipoOperacion.Ingesos)
                .Sum(x => x.Monto);

            //buscamos el balance de los retiros o gastos. 
            public decimal BalanceRetiros => Transacciones
                .Where(x => x.TipoOperacionId == TipoOperacion.Gastos)
                .Sum(x => x.Monto);
        }
    }
}
