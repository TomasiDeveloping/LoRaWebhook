using AutoMapper;
using Core.DataTransferObjects;
using Database.Entities;

namespace Core.Profiles;

public class SensorTypeProfile : Profile
{
    public SensorTypeProfile()
    {
        CreateMap<SensorType, SensorTypeDto>();
        CreateMap<SensorTypeDto, SensorType>()
            .ForMember(des => des.CreatedAt, opt => opt.Ignore());
    }
}