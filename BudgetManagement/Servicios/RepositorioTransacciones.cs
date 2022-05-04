using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;


namespace BudgetManagement.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Crear(Transaccion transaction);
    }
    public class RepositorioTransacciones : IRepositorioTransacciones
    {
        private readonly string connectionString;
        public RepositorioTransacciones(IConfiguration configuration)
        {

            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        

        public async Task Crear(Transaccion transaccion) 
        {
            using var connection =  new  SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"Transacciones_Insertar", 
                                       new {transaccion.UsuarioId,
                                            transaccion.FechaTransaccion,
                                            transaccion.Monto,
                                            transaccion.CategoriaId,
                                            transaccion.CuentasId,
                                            transaccion.Nota},
                                            commandType: System.Data.CommandType.StoredProcedure);
            transaccion.Id = id;
        }
    }
}
