using System.Runtime.Serialization;

namespace HomeAutomationFunctions.Notifications
{

    public class GcmNotificationPayload : NotificationPayloadBase
    {
        public GcmNotificationPayload(string message) : base(message) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context) =>
            info.AddValue("data", new { message = Message });
    }
}
