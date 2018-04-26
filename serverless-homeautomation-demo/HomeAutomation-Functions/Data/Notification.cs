using System.Collections.Generic;
using HomeAutomationFunctions.Data;
using Newtonsoft.Json;

namespace HomeAutomationFunctions.Functions.Data
{
    public class Notification
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("data")]
        public dynamic Data { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("badge")]
        public string Badge { get; set; }
        [JsonProperty("actions")]
        public List<ActionData> Actions { get; set; }
    }
}