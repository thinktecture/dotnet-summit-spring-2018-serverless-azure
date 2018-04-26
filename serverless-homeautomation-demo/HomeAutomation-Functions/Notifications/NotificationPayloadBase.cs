using System.Runtime.Serialization;

namespace HomeAutomationFunctions.Notifications
{

    public abstract class NotificationPayloadBase : ISerializable
    {
        protected NotificationPayloadBase(string message) => Message = message;

        public string Message { get; private set; }

        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
    }
}
