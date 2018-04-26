using Newtonsoft.Json;

namespace HomeAutomationFunctions.Data
{
    public class Alert
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
