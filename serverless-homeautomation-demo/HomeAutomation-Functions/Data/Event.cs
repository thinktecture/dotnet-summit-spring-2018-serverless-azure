using System;
using Newtonsoft.Json;

namespace HomeAutomationFunctions.Data
{
    public class Event
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("EventType")]
        public string EventType { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        [JsonProperty("eventTime")]
        public DateTime EventTime { get; set; }
        [JsonProperty("data")]
        public Alert Data { get; set; }
    }
}
