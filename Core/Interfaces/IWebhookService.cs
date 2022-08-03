using Newtonsoft.Json.Linq;

namespace Core.Interfaces;

public interface IWebhookService
{
    Task<bool> ProcessingSensorData(JObject jObject);
}