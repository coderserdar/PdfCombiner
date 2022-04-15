using System;

namespace PdfCombiner
{
    /// <summary>
    /// This function is used to take exceptions which can be thrown in
    /// PDF Combiner App
    /// </summary>
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
}
