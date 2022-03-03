using System;

namespace PdfCombiner
{
    [Serializable]
    public abstract class BaseException : Exception
    {
        protected BaseException()
        {
        }

        protected BaseException(string message) : base(message)
        {
        }

        protected BaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public static class ExceptionExtensions
    {
        public static string GetAllMessages(this Exception exception)
        {
            var messages = exception.Message;
            if (exception.InnerException != null)
                messages += " " + exception.InnerException.GetAllMessages();
            if (!string.IsNullOrEmpty(exception.StackTrace))
                messages += " " + exception.StackTrace;
            if (!string.IsNullOrEmpty(exception.Source))
                messages += " " + exception.Source;
            return messages;
        }
    }
}
