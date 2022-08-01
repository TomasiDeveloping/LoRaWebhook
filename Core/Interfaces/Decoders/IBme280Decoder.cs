using Newtonsoft.Json.Linq;

namespace Core.Interfaces.Decoders;

public interface IBme280Decoder
{
    Task<bool> CreateMeasurement(JObject jObject);
}