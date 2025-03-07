using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Registro.DAL.DBContext;
using Registro.Utility;
using Registro.DAL.Repository.IRepository;
using Registro.DAL.Repository;
using Registro.BLL.Services.ServicesContracts;
using Registro.BLL.Services;


namespace Registro.IOC
{
    public static class Dependencia
    {
        //Método para inyectar las dependencias
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            //CONEXIÓN A LA BASE DE DATOS
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });

            //DEPENDENCIAS DE LOS REPOSITORIOS DE LA CAPA DE DATOS
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //DEPENDENCIAS DE LAS UTILERÍAS DE LA CAPA DE UTILIDADES PARA EL MAPPING
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //Dependencias de los servicios
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IPeliculaService, PeliculaService>();
        }
    }
}
