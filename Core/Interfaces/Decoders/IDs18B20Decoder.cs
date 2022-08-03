using Core.DataTransferObjects;
using Newtonsoft.Json.Linq;

namespace Core.Interfaces.Decoders;

public interface IDs18B20Decoder
{
    Task<bool> CreateMeasurement(JObject jObject, SensorDto sensorDto);
}