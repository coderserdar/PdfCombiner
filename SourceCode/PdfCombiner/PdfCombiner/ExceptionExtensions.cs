using System;
using System.Text;

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
            var sb = new StringBuilder();
            sb.Append(exception.Message);
            if (exception.InnerException != null)
                sb.Append($" {exception.InnerException.GetAllMessages()}");
            if (!string.IsNullOrEmpty(exception.StackTrace))
                sb.Append($" {exception.StackTrace}");
            if (!string.IsNullOrEmpty(exception.Source))
                sb.Append($" {exception.Source}");
            return sb.ToString();
        }
    }
}
