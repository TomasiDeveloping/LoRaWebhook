using Core.DataTransferObjects;
using Core.Interfaces.Decoders;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using static System.Decimal;

namespace Core.Helpers.Decoders;

public class Ds18B20Decoder : IDs18B20Decoder
{
    private readonly IDs18B20MeasurementRepository _deDs18B20MeasurementRepository;
    private readonly ILogger<Ds18B20Decoder> _logger;

    public Ds18B20Decoder(IDs18B20MeasurementRepository deDs18B20MeasurementRepository, ILogger<Ds18B20Decoder> logger)
    {
        _deDs18B20MeasurementRepository = deDs18B20MeasurementRepository;
        _logger = logger;
    }

    public async Task<bool> CreateMeasurement(JObject jObject, SensorDto sensorDto)
    {
        var ds18B20Helper = GetValues(jObject);

        if (ds18B20Helper == null)
        {
            _logger.LogError("Not all data was transmitted!");
            return false;
        }

        await InsertMeasurement(ds18B20Helper, sensorDto.Id);

        return true;
    }

    private async Task InsertMeasurement(Ds18B20Helper ds18B20Helper, Guid sensorId)
    {
        var ds18B20Measurement = new Ds18B20MeasurementDto
        {
            SensorId = sensorId,
            Temperature = ds18B20Helper.Temperature,
            SendTime = ds18B20Helper.SendTime
        };
        await _deDs18B20MeasurementRepository.InsertAsync(ds18B20Measurement);
    }

    private static Ds18B20Helper? GetValues(JToken jObject)
    {
        var temperatureString = jObject.SelectToken("uplink_message.decoded_payload.temperature")?.ToString();
        var sendTimeString = jObject.SelectToken("received_at")?.ToString();

        if (string.IsNullOrEmpty(temperatureString) || string.IsNullOrEmpty(sendTimeString)) return null;

        return new Ds18B20Helper
        {
            SendTime = DateTime.Parse(sendTimeString),
            Temperature = Parse(temperatureString)
        };
    }

    private class Ds18B20Helper
    {
        public decimal Temperature { get; init; }
        public DateTime SendTime { get; init; }
    }
}