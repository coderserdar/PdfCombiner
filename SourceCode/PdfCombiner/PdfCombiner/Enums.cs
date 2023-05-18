using System;

namespace PdfCombiner
{
    /// <summary>
    /// This class contains enums for detailed information
    /// In usage
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// This enum is used for
        /// Assigning the order type in listbox items
        /// Ascending or Descending
        /// </summary>
        [Flags]
        public enum OrderType
        {
            Ascending,
            Descending
        }
        
        /// <summary>
        /// This enum is used for
        /// Assigning the sort type in listbox items
        /// By Name or By Path
        /// </summary>
        [Flags]
        public enum SortType
        {
            ByName,
            ByPath
        }
    }
}