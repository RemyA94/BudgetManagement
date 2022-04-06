namespace BudgetManagement.Servicios
{
    public interface IRepositorioTransacciones 
    {
    
    }
    public class RepositorioTransacciones : IRepositorioTransacciones
    {
        private readonly string connectionString;
        public RepositorioTransacciones(IConfiguration configuration)
        {

            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
