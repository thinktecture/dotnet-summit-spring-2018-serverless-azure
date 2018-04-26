using HomeAutomationFunctions.Functions.Data;
using Newtonsoft.Json;

namespace HomeAutomationFunctions.Data
{
    public class PushData
    {
        [JsonProperty("notification")]
        public Notification Notification { get; set; }
    }
}