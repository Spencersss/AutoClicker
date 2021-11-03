using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpencerAutoClicker
{
    class Clicker
    {
        // Config
        public Key Hotkey_Mouse_Click { get; set; } // key used to toggle clicker
        public Key Hotkey_Mouse_Down { get; set; } // key used to toggle mouse down
        public int ClickInterval { get; set; } // in milliseconds

        // States
        public bool ClickerRunning { get; set; } // left click running
        public bool HoldDownRunning { get; set; } // hold left click
        private Thread ControlLoopThread;

        // Fields 
        private Process _currentProcess { get; set; } // current process clicker interacts with

        // Property
        public string ProcessWindowTitle => _currentProcess.MainWindowTitle;

        // Enums
        public enum MouseButton
        {
            LeftDown = 0x201,
            LeftUp = 0x202
        }

        // Constructor
        public Clicker()
        {
            // Init vals
            Hotkey_Mouse_Click = Key.F9;
            Hotkey_Mouse_Down = Key.F10;
            ClickInterval = 100;
            ClickerRunning = false;
            HoldDownRunning = false;
            ControlLoopThread = null;
            _currentProcess = null;
        }

        public void SetProcess(Process proc)
        {
            _currentProcess = proc;
        }

        public Process GetProcess()
        {
            return _currentProcess;
        }

        //bits 0-15 = repeat count (2 bytes)
        //bits 16-23 = scan code (1 byte)
        //bit 24 = extended key (1 bit)
        //bits 25-28 = reserved, do not use.  (4 bits)
        //bit 29 = context code (0 for WM_KEYDOWN, 1 for WM_KEYUP)
        //bit 30 = previous keystate, 1 = down, 0 = up
        //bit 31 = transition state, 0 for WM_KEYDOWN, 1 for WM_KEYUP)
        public uint GenLParams(uint key, int x, int y)
        {
            uint lParams = 0x000000000;

            lParams |= Natives.MapVirtualKeyEx(key, 0, Natives.GetKeyboardLayout(0));
            lParams |= (uint)((y << 16) | (x & 0xFFFF));

            return lParams;
        }

        private void ClickerControlLoop()
        {
            IntPtr procWindow = Natives.FindWindow(null, ProcessWindowTitle);
            Natives.Rect winRectangle = new Natives.Rect();
            bool gotRectangle = Natives.GetWindowRect(procWindow, ref winRectangle);

            uint mouseLeftDown = Natives.MapVirtualKeyEx((uint)MouseButton.LeftDown, 0, Natives.GetKeyboardLayout(0));
            uint mouseLeftUp = Natives.MapVirtualKeyEx((uint)MouseButton.LeftUp, 0, Natives.GetKeyboardLayout(0));

            if (gotRectangle)
            {
                int x = (winRectangle.Right - winRectangle.Left) / 2;
                int y = (winRectangle.Bottom - winRectangle.Top) / 2;
                while (ClickerRunning)
                {
                    Natives.PostMessage(procWindow, 0x0201, mouseLeftDown, GenLParams((uint)MouseButton.LeftDown, x, y));
                    Thread.Sleep(10);
                    Natives.PostMessage(procWindow, 0x0202, mouseLeftUp, GenLParams((uint)MouseButton.LeftUp, x, y));
                    Thread.Sleep(ClickInterval);
                }
            }
        }

        // Methods
        public void StartClicker()
        {
            if (!ClickerRunning && _currentProcess != null)
            {
                ClickerRunning = true;
                ControlLoopThread = new Thread(ClickerControlLoop);
                ControlLoopThread.Start();
            }
        }

        public void StopClicker()
        {
            if (ClickerRunning)
            {
                ClickerRunning = false;
            }
        }

        public void StartMouseDown()
        {
            if (!HoldDownRunning && _currentProcess != null)
            {
                HoldDownRunning = true;
            }
        }

        public void StopMouseDown()
        {
            if (HoldDownRunning)
            {
                HoldDownRunning = false;
            }
        }

        public bool IsProcessSelected()
        {
            return _currentProcess != null;
        }

    }
}
