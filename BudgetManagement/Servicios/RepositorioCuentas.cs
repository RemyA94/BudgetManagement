using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Crear(Cuenta cuenta);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {
        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Cuenta cuenta) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"Insert Into Cuentas (Nombre, TipoCuentaId, Descripcion, Balance)
                 Values (@Nombre, @TipoCuentaId, @Descripcion, @Balance);
                 Select SCOPE_IDENTITY();", cuenta);

            cuenta.Id = id;
            
        }
    }
}
