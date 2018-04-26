using Newtonsoft.Json;
using System.Collections.Generic;

namespace HomeAutomation.Common
{
    public class Device
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Room
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("devices")]
        public List<Device> Devices { get; set; }
    }

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
    }

    public class GetBuildingInfoResponse
    {
        [JsonProperty("rooms")]
        public List<Room> Rooms { get; set; }
    }

    public class ControlClimateDataRequest
    {
        [JsonProperty("device")]
        public string Device { get; set; }
        [JsonProperty("command")]
        public string Command { get; set; }

    }

    public class ControlBlindsRequest
    {
        [JsonProperty("device")]
        public string Device { get; set; }
        [JsonProperty("command")]
        public string Command { get; set; }
    }

    public class ControlBlindsResponse
    {
        [JsonProperty("device")]
        public string Device { get; set; }
        [JsonProperty("state")]
        public bool State { get; set; }
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }
    }

    public class GetDeviceStateRequest
    {
        [JsonProperty("device")]
        public string Device { get; set; }
    }

    public class GetDeviceStateResponse
    {
        [JsonProperty("device")]
        public string Device { get; set; }
        [JsonProperty("position")]
        public int Position { get; set; }
    }
}