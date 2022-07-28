using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;


namespace BudgetManagement.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Borrar(int id);
        Task Crear(Transaccion transaction);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
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

        public async Task<IEnumerable<Transaccion>>ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaccion>
                (@"Select t.Id, t.Monto, t.FechaTransaccion,
                    c.Nombre as Categoria, cu.Nombre as Cuenta, c.TipoOperacionId
                    from Transacciones t
                    Inner Join Categorias c
                    on c.Id = t.CategoriaId
                    Inner Join Cuentas cu
                    on cu.Id = t.CuentaId
                    Where t.CuentaId = @CuentasId and t.UsuarioId = @UsuarioId
                    and FechaTransaccion between @FechaInicio and @FechaFin
                    ", modelo);
        }


        public async Task Actualizar(Transaccion transaccion, decimal montoAnterior, 
            int cuentaAnteriorId)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.ExecuteAsync(@"Transacciones_Actualizar",
                                               new 
                                               {
                                                   transaccion.Id,
                                                   transaccion.FechaTransaccion,
                                                   transaccion.Monto,
                                                   transaccion.CategoriaId,
                                                   transaccion.CuentasId,
                                                   transaccion.Nota,
                                                   montoAnterior,
                                                   cuentaAnteriorId
                                               }, commandType:System.Data.CommandType.StoredProcedure);
        }

        public async Task<Transaccion> ObtenerPorId(int id, int usuarioId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transaccion>(
                            @"SELECT Transacciones.*, cat.TipoOperacionId
                            FROM Transacciones
                            INNER JOIN Categorias cat
                            ON cat.Id = Transacciones.CategoriaId
                            WHERE Transacciones.Id = @Id AND Transacciones.UsuarioId = @UsuarioId",
                            new { id, usuarioId });
        }

        public async Task Borrar(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                            @"Transacciones_Borrar", new {id},
                            commandType: System.Data.CommandType.StoredProcedure);
        }
           
    }
}
