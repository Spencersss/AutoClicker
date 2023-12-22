using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpencerAutoClicker.Source.Backend.Exceptions
{
    public class InputNotFoundException : Exception
    {
        public InputNotFoundException() { } 

        public InputNotFoundException(string inputVal) 
            : base(inputVal) { }
    }
}
