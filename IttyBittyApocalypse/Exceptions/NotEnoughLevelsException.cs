using System;

namespace IttyBittyApocalypse
{
    public class NotEnoughLevelsException : Exception
    {
        public NotEnoughLevelsException() { }
        public NotEnoughLevelsException(string message) : base(message) { }
        public NotEnoughLevelsException(string message, Exception inner) : base(message, inner) { }
    }
}
