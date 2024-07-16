using System;

namespace SpencerAutoClicker.Source.Model.Exceptions
{
    public class InputNotFoundException : Exception
    {
        public InputNotFoundException() { }

        public InputNotFoundException(string inputVal)
            : base(inputVal) { }
    }
}
