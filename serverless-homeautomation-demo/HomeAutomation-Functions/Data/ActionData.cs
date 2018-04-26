using Newtonsoft.Json;

namespace HomeAutomationFunctions.Data
{
    public class ActionData
    {
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
    }
}