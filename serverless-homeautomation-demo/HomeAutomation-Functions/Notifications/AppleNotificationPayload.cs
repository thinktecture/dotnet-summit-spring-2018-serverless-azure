using System.Runtime.Serialization;

namespace HomeAutomationFunctions.Notifications
{

    public class AppleNotificationPayload : NotificationPayloadBase
    {
        public AppleNotificationPayload(string message) : base(message) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) =>
            info.AddValue("aps", new { alert = Message });
    }
}
