using AutoMapper;
using BudgetManagement.Models;

namespace BudgetManagement.Servicios
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
            CreateMap<TransaccionActulizacionViewModel, Transaccion>().ReverseMap();
        }
    }
}
