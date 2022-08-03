using AutoMapper;
using Core.DataTransferObjects;
using Database.Entities;

namespace Core.Profiles;

public class Ds18B20MeasurementProfile : Profile
{
    public Ds18B20MeasurementProfile()
    {
        CreateMap<Ds18B20Measurement, Ds18B20MeasurementDto>()
            .ForMember(des => des.DevAddress, opt => opt.MapFrom(src => src.Sensor.DevAddress));
        CreateMap<Ds18B20MeasurementDto, Ds18B20Measurement>();
    }
}