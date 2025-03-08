using AutoMapper;
using Registro.DTO;
using Registro.Model;
using Registro.Model.Entities;

namespace Registro.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion Rol 

            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion Menu

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino =>//destino:UsuarioDTO
                destino.RolDescripcion,
                opt => opt.MapFrom(origen => origen.IdRolNavigation !=null ? origen.IdRolNavigation.Nombre : null))//origen:Usuario
                
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0)
                );

            CreateMap<Usuario, SesionDTO>()
                .ForMember(destino =>
                destino.RolDescripcion,
                opt => opt.MapFrom(origen => origen.IdRolNavigation !=null ? origen.IdRolNavigation.Nombre : null)
                );

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino =>
                destino.IdRolNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false)
                );
            #endregion Usuario

            #region Pelicula
            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            #endregion Pelicula

            #region Comentario
            CreateMap<Comentario, ComentarioDTO>().ReverseMap();
            #endregion Comentario

        }

    }
}
