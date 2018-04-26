using System;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace HomeAutomationFunctions.Data
{
    public class ClimateData
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
        [JsonProperty("messageId")]
        public int MessageId { get; set; }
        [JsonProperty("temperature")]
        public int Temperature { get; set; }
        [JsonProperty("humidity")]
        public int Humidity { get; set; }
        [JsonProperty("dateTime")]
        public string DateTime { get; set; }
        [JsonConverter(typeof(UnixDateTimeConverter))]
        [JsonProperty(PropertyName = "_ts")]
        public DateTime Timestamp { get; }
    }
}

