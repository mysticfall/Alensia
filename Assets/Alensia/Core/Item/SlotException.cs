using System;

namespace Alensia.Core.Item
{
    public class SlotException : Exception
    {
        public SlotException()
        {
        }

        public SlotException(string message) : base(message)
        {
        }

        public SlotException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}