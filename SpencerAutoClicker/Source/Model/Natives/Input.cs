using System.Runtime.InteropServices;

namespace SpencerAutoClicker.Source.Model.Natives
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Input
    {
        internal uint type;
    }
}
