using System;
using System.Runtime.InteropServices;

namespace SpencerAutoClicker.Source.Model.Natives
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseInput
    {
        internal long X;
        internal long Y;
        internal uint MouseData;
        internal uint Flags;
        internal uint Time;
        internal UIntPtr ExtraInfo;
    }
}
