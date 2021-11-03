using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Natives
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
