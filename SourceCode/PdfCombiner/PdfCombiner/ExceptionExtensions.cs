using System;

namespace PdfCombiner
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// This method is used to take all messages with stack trace info in exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>Detailed Error Message</returns>
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
