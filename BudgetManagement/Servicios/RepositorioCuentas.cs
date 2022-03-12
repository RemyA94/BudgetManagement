using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Servicios
{
    public interface IRepositorioCuentas
    {
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
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
                @"Insert Into Cuentas (Nombre, TipoCuentasId, Descripcion, Balance)
                 Values (@Nombre, @TipoCuentasId, @Descripcion, @Balance);
                 Select SCOPE_IDENTITY();", cuenta);

            cuenta.Id = id;
            
        }
        public async Task<IEnumerable<Cuenta>> Buscar(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Cuenta>(@"Select Cuentas.Id, Cuentas.Nombre, Cuentas.Balance, tc.Nombre as TipoCuenta
                                                        from Cuentas
                                                        Inner Join TipoCuentas as tc 
                                                        on tc.Id = Cuentas.TipoCuentasId
                                                        where tc.UsuarioId = @UsuarioId
                                                        order by tc.Orden", new {usuarioId});
        }
    }
}
