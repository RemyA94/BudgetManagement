using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);

        //Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
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
                                                            (@"Insert Into TipoCuentas (Nombre, UsuarioId, Orden)
                                                                Values (@Nombre, @UsuarioId,0);
                                                                Select Scope_Identity();", tipoCuenta);
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
                                                                Where UsuarioId = @UsuarioId;", new { usuarioId });
           
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

        //public async Task Borrar(int id) 
        //{
        //    using var connection = new SqlConnection(connectionString);
        //    await connection.ExecuteAsync("Delete TipoCuentas where Id = @Id;", new { id });
        
        //}


    }
}
