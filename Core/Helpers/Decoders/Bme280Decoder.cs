using Core.DataTransferObjects;
using Core.Interfaces.Decoders;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using static System.Decimal;

namespace Core.Helpers.Decoders;

public class Bme280Decoder : IBme280Decoder
{
    private readonly IBme280MeasurementRepository _bme280MeasurementRepository;
    private readonly ILogger<Bme280Decoder> _logger;

    public Bme280Decoder(IBme280MeasurementRepository bme280MeasurementRepository, ILogger<Bme280Decoder> logger)
    {
        _bme280MeasurementRepository = bme280MeasurementRepository;
        _logger = logger;
    }

    public async Task<bool> CreateMeasurement(JObject jObject, SensorDto sensor)
    {
        var bme280Helper = GetValues(jObject);

        if (bme280Helper == null)
        {
            _logger.LogError("Not all data was transmitted!");
            return false;
        }

        await InsertMeasurement(bme280Helper, sensor.Id);

        return true;
    }

    private async Task InsertMeasurement(Bme280Helper bme280Helper, Guid sensorId)
    {
        var bmeMeasurement = new Bme280MeasurementDto
        {
            SensorId = sensorId,
            Humidity = bme280Helper.Humidity,
            Pressure = bme280Helper.Pressure,
            Temperature = bme280Helper.Temperature,
            SendTime = bme280Helper.SendTime
        };
        await _bme280MeasurementRepository.InsertAsync(bmeMeasurement);
    }

    
    private static Bme280Helper? GetValues(JToken jObject)
    {
        var temperatureString = jObject.SelectToken("uplink_message.decoded_payload.temperature")?.ToString();
        var humidityString = jObject.SelectToken("uplink_message.decoded_payload.humidity")?.ToString();
        var pressureString = jObject.SelectToken("uplink_message.decoded_payload.airpressure")?.ToString();
        var sendTimeString = jObject.SelectToken("received_at")?.ToString();

        if (string.IsNullOrEmpty(temperatureString) || string.IsNullOrEmpty(humidityString) ||
            string.IsNullOrEmpty(pressureString) || string.IsNullOrEmpty(sendTimeString)) return null;

        return new Bme280Helper
        {
            SendTime = DateTime.Parse(sendTimeString),
            Humidity = Parse(humidityString),
            Pressure = Parse(pressureString),
            Temperature = Parse(temperatureString)
        };
    }

    private class Bme280Helper
    {
        public decimal Temperature { get; init; }
        public decimal Humidity { get; init; }
        public decimal Pressure { get; init; }
        public DateTime SendTime { get; init; }
    }
}