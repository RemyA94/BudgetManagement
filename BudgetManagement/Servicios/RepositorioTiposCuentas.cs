using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Crear(TipoCuenta tipoCuenta);
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
            var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>
                                                            ($@"Insert Into TipoCuentas (Nombre, UsuarioId, Orden)
                                                                Values (@Nombre, @UsuarioId,0);
                                                                Select Scope_Identity();", tipoCuenta);
            tipoCuenta.Id = id;
        }
    }
}
