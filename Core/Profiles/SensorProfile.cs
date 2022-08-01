using AutoMapper;
using Core.DataTransferObjects;
using Database.Entities;

namespace Core.Profiles;

public class SensorProfile : Profile
{
    public SensorProfile()
    {
        CreateMap<Sensor, SensorDto>()
            .ForMember(des => des.SensorTypeName, opt => opt.MapFrom(src => src.SensorType.Name));
        CreateMap<SensorDto, Sensor>()
            .ForMember(des => des.UpdatedAt, opt => opt.Ignore());
    }
}