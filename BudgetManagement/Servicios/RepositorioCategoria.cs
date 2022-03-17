using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Crear(Categoria categoria);
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
            var id = await connection.QuerySingleAsync<int>(@"Insert into  Categorias (Nombre, TipoOperacionId,UsuarioId)
                                                        values (@Nombre, @TipoOperacionId, @UsuarioId)
                                                        Select SCOPE_IDENTITY() ;", categoria);

            categoria.Id = id;
        }

    }
}
