using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCategoria: IRepositorioCategorias
    {
        private readonly string connectionString;
        public RepositorioCategoria(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Categoria categoria) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                                      Insert into  Categorias 
                                      (Nombre, TipoOperacionId,UsuarioId)
                                      values (@Nombre, @TipoOperacionId, @UsuarioId)
                                      Select SCOPE_IDENTITY() ;", categoria);

            categoria.Id = id;
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(
                @"Select * from Categorias Where UsuarioId = @usuarioId", new { usuarioId });
        }
        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(
                @"Select * from Categorias
                Where UsuarioId = @usuarioId
                and TipoOperacionId = @tipoOperacionId", new { usuarioId, tipoOperacionId});
        }

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connetion = new SqlConnection(connectionString);
            return await connetion.QueryFirstOrDefaultAsync<Categoria>(
                @"Select * from Categorias where Id = @Id and UsuarioId = @usuarioId", 
                new { id, usuarioId });
        }

        public async Task Actualizar(Categoria categoria) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
            Update Categorias set Nombre=@Nombre, 
            TipoOperacionId =@tipoOperacionId
            Where Id = @Id ", categoria);
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"Delete Categorias where Id=@Id;", new {id});
        }
    }
}
