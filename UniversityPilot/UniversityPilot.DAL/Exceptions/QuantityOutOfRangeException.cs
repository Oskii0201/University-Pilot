namespace UniversityPilot.DAL.Exceptions
{
    using System;

    public class QuantityOutOfRangeException : Exception
    {
        public QuantityOutOfRangeException()
        {
        }

        public QuantityOutOfRangeException(string message) : base(message)
        {
        }

        public QuantityOutOfRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}