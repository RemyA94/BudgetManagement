using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
        Task Odenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados);
    }
    public class RepositorioTiposCuentas: IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {

            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(TipoCuenta tipoCuenta) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                                                            (@"TipoCuentas_Insertar",
                                                            new {UsuarioId = tipoCuenta.UsuarioId,
                                                            Nombre = tipoCuenta.Nombre},
                                                            commandType: System.Data.CommandType.StoredProcedure);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId) 
        {
            using var connetion = new SqlConnection(connectionString);
            var existe = await connetion.QueryFirstOrDefaultAsync<int>(@"Select 1 from TipoCuentas
                                                                      where Nombre = @Nombre and UsuarioId = @UsuarioId;",
                                                                      new {nombre, usuarioId});
            return existe == 1;
        
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"Select Id, Nombre, Orden
                                                                from TipoCuentas
                                                                Where UsuarioId = @UsuarioId
                                                                Order by Orden;", new { usuarioId });
           
        }

        public async Task Actualizar (TipoCuenta tipoCuenta) 
        {
            using var connection= new SqlConnection(connectionString);
            await connection.ExecuteAsync("Update TipoCuentas Set Nombre = @Nombre Where Id = @Id;", tipoCuenta);
        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"Select Id, Nombre, Orden
                                                                from TipoCuentas
                                                                Where Id = @Id and usuarioId = @UsuarioId;", 
                                                                new { id, usuarioId });

        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Delete TipoCuentas where Id = @Id;", new { id });

        }

        public async Task Odenar(IEnumerable<TipoCuenta> tipoCuentasOrdenados) 
        {
            var query = "update TipoCuentas set Orden = @Orden where Id = @Id;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, tipoCuentasOrdenados);
        }

    }
}
