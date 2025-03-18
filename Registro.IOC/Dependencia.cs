using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Registro.DAL.DBContext;
using Registro.Utility;
using Registro.BLL.Services.ServicesContracts;
using Registro.BLL.Services;
using Registro.BLL.Repository;
using Registro.BLL.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtKey = configuration["Jwt:Key"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        
                        ValidateIssuer = false, 
                        ValidateAudience = false, 
                        ValidateLifetime = true,  
                        ValidateIssuerSigningKey = true,  
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey!))
                    };
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
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
