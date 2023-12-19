using AutoMapper;
using IOTBackend.API.BusinessLogicLayer.DTO;
using IOTBackend.API.DataLayer.Entities;

namespace IOTBackend.API.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<NewUserRequestModel, User>()
                .ForMember(m => m.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(m => m.Password, opt => opt.MapFrom(src => EasyEncryption.MD5.ComputeMD5Hash(src.Password)));


            CreateMap<UserRequestModel, User>();


            CreateMap<RoleRequestModel, Role>()
                .ForMember(m => m.CreatedOn, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<User, UserResponseModel>();



            CreateMap<Role, RoleResponseModel>();
        }


    }
}
