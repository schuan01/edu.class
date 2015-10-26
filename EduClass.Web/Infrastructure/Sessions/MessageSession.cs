using System;
using System.Web;

namespace EduClass.Web.Infrastructure.Sessions
{
    public class MessageSession
    {
        private static string sessionMessage = "educlass_session_message";

        public static void SetMessage(MessageHelper message)
        {
            HttpContext.Current.Session[sessionMessage] = message;
        }

        public static string GetMessage()
        {
            if (HttpContext.Current.Session[sessionMessage] != null)
            {
                var msg = (MessageHelper)HttpContext.Current.Session[sessionMessage];

                HttpContext.Current.Session[sessionMessage] = null;

                return msg.ToString();
            }

            return "";
        }
    }

    public class MessageHelper
    {
        public Enum_MessageType Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Type Caller { get; set; }
        public Exception Exception { get; set; }

        public MessageHelper(Enum_MessageType status, string title, string description)
        {
            this.Status = status;
            this.Title = title;
            this.Description = description;
        }

        public MessageHelper(Enum_MessageType status, string title, string description, Type caller, Exception exception)
        {
            this.Status = status;
            this.Title = title;
            this.Description = description;
            this.Caller = caller;
            this.Exception = exception;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Status.ToString().ToLower(), Title, Description);
        }
    }

    public enum Enum_MessageType
    {
        INFO = 1,
        DANGER = 2,
        WARNING = 3,
        SUCCESS = 4
    }
}