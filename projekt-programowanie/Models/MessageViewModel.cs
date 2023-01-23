using Microsoft.Extensions.Primitives;

namespace projekt_programowanie.Models
{
    public enum MessageType
    {
        Success,
        Error
    }

    public class MessageViewModel
    {
        public MessageViewModel(string message, MessageType messageType, string redirect_ControllerName, string redirect_ActionName)
        {
            Message = message;
            MessageType = messageType;
            Redirect_ControllerName= redirect_ControllerName;
            Redirect_ActionName= redirect_ActionName;
        }

        public string Message { get; }
        public MessageType MessageType { get; }
        public string Redirect_ControllerName { get; }
        public string Redirect_ActionName { get; }
    }
}
