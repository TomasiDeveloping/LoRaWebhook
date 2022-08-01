using AutoMapper;
using Core.DataTransferObjects;
using Database.Entities;

namespace Core.Profiles;

public class Bme280MeasurementProfile : Profile
{
    public Bme280MeasurementProfile()
    {
        CreateMap<Bme280Measurement, Bme280MeasurementDto>()
            .ForMember(des => des.DevAddress, opt => opt.MapFrom(src => src.Sensor.DevAddress));
        CreateMap<Bme280MeasurementDto, Bme280Measurement>();
    }
}