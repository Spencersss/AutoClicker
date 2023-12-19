using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SpencerAutoClicker.Source.Backend.Natives
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Input
    {
        internal uint type;
    }
}
