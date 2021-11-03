using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Natives
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
