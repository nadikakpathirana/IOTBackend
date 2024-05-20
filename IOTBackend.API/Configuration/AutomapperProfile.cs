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
            
            CreateMap<DeviceUpdateDto, Device>();
            
            CreateMap<APIKeyAddDto, APIKey>();
            CreateMap<APIKeyUpdateDto, APIKey>();
        }
        
    }
}
