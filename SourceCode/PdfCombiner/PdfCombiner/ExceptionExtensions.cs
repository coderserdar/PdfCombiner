using System;
using System.Text;

namespace PdfCombiner
{
    /// <summary>
    /// This class is used to take generic exception messages
    /// With the inner exception, stack trace and source informations
    /// </summary>
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
            sb.AppendLine(exception.Message);
            if (exception.InnerException != null)
                sb.AppendLine($" {exception.InnerException.GetAllMessages()}");
            if (!string.IsNullOrEmpty(exception.StackTrace))
                sb.AppendLine($" {exception.StackTrace}");
            if (!string.IsNullOrEmpty(exception.Source))
                sb.AppendLine($" {exception.Source}");
            return sb.ToString();
        }
    }
}
