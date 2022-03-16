using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Servicios
{
    public interface IRepositorioCuentas
    {
        Task Actualizar(CuentaCreacionViewModel cuenta);
        Task Borrar(int id);
        Task<IEnumerable<Cuenta>> Buscar(int usuarioId);
        Task Crear(Cuenta cuenta);
        Task<Cuenta> ObtenerPorId(int id, int usuarioId);
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
        public async Task<Cuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Cuenta>(@"Select Cuentas.Id, Cuentas.Nombre, Cuentas.Balance, Descripcion, tc.Id
                                                        from Cuentas
                                                        Inner Join TipoCuentas as tc 
                                                        on tc.Id = Cuentas.TipoCuentasId
                                                        where tc.UsuarioId = @UsuarioId and Cuentas.Id = @Id", new {id, usuarioId });
        }
        public async Task Actualizar(CuentaCreacionViewModel cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update Cuentas
                                                set Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion,
                                                TipoCuentasId = @TipoCuentasId
                                                where Cuentas.Id = @id;", cuenta);
        }

        public async Task Borrar(int id)
        {
            var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"Delete cuentas where id =@id;", new {id});
        }
    }
}
