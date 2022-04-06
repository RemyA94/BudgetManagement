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
        

        public async Task Crear(Transaccion transaction) 
        {
            using var connection =  new  SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"Transacciones_Insertar", 
                                       new {transaction.UsuarioId, transaction.Monto,
                                            transaction.FechaTransacion, transaction.CategoriaId,
                                            transaction.Nota, transaction.CuentaId},
                                            commandType: System.Data.CommandType.StoredProcedure);
            transaction.Id = id;
        }
    }
}
