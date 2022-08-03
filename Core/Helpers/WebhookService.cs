using Core.DataTransferObjects;
using Core.Interfaces;
using Core.Interfaces.Decoders;
using Core.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Core.Helpers;

public class WebhookService : IWebhookService
{
    private readonly ILogger<WebhookService> _logger;
    private readonly ISensorRepository _sensorRepository;
    private readonly IBme280Decoder _bme280Decoder;
    private readonly IDs18B20Decoder _ds18B20Decoder;

    public WebhookService(ILogger<WebhookService> logger, ISensorRepository sensorRepository, IBme280Decoder bme280Decoder, IDs18B20Decoder ds18B20Decoder)
    {
        _logger = logger;
        _sensorRepository = sensorRepository;
        _bme280Decoder = bme280Decoder;
        _ds18B20Decoder = ds18B20Decoder;
    }

    public async Task<bool> ProcessingSensorData(JObject jObject)
    {
        var devAddress = jObject.SelectToken("end_device_ids.dev_addr")?.ToString();
        if (devAddress == null)
        {
            _logger.LogError("DevAddress in JObject is empty!");
            return false;
        }

        var sensor = await GetSensor(devAddress);
        if (sensor == null)
        {
            _logger.LogError($"No sensor found with devAddress: {devAddress}");
            return false;
        }

        switch (sensor.SensorTypeName?.ToUpper())
        {
            case "BME280":
                return await _bme280Decoder.CreateMeasurement(jObject, sensor);
            case "DS18B20":
                return await _ds18B20Decoder.CreateMeasurement(jObject, sensor);
            default:
                _logger.LogError($"Error in switch sensor types for type: {sensor.SensorTypeName}");
                return false;
        }
    }

    private async Task<SensorDto?> GetSensor(string devAddress)
    {
        var sensor = await _sensorRepository.GetSensorByDevAddressAsync(devAddress);
        return sensor;
    }
}