using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Registro.DAL.DBContext;
using Registro.Utility;
using Registro.BLL.Services.ServicesContracts;
using Registro.BLL.Services;
using Registro.BLL.Repository;
using Registro.BLL.Repository.IRepository;



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



            
            

            //DEPENDENCIAS DE LAS UTILERÍAS DE LA CAPA DE UTILIDADES PARA EL MAPPING
            services.AddAutoMapper(typeof(AutoMapperProfile));


            //DEPENDENCIAS DE LOS REPOSITORIOS DE LA CAPA DE NEGOCIOS
            //Dependencias de los servicios
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IPeliculaService, PeliculaService>();
            services.AddScoped<IComentarioService, ComentarioService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();

            //Repositorios Genericos
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
