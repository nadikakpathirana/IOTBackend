using AutoMapper;
using IOTBackend.Domain.DbEntities;
using IOTBackend.Domain.Dtos;

namespace IOTBackend.API.Configuration
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserUpdateDto, User>();
            
            CreateMap<DeviceUpdateDto, Device>();
            
            CreateMap<ApiKeyAddDto, ApiKey>();
            CreateMap<ApiKeyUpdateDto, ApiKey>();

            CreateMap<ConnectionLineUpdateDto, ConnectionLine>();
            
            CreateMap<ProjectCreateDto, Project>();
            CreateMap<ProjectUpdateDto, Project>();
            
            CreateMap<DeviceInstanceCreateDto, DeviceInstance>();
            CreateMap<DeviceInstanceUpdateDto, DeviceInstance>();
        }
        
    }
}
