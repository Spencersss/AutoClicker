using System;
using Natives;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SpencerAutoClicker
{
    public class Clicker
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
        private Process CurrentProcess { get; set; } // current process clicker interacts with

        // Property
        public string ProcessWindowTitle => CurrentProcess.MainWindowTitle;

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
            CurrentProcess = null;
        }

        public void SetProcess(Process proc)
        {
            CurrentProcess = proc;
        }

        public Process GetProcess()
        {
            return CurrentProcess;
        }

        /*
         
        Getting scan codes of left mouse button up/down
          
        uint mouseLeftDown = NativeMethods.MapVirtualKeyEx(
            (uint)MouseButton.LeftDown, 0, NativeMethods.GetKeyboardLayout(0));
        uint mouseLeftUp = NativeMethods.MapVirtualKeyEx(
            (uint)MouseButton.LeftUp, 0, NativeMethods.GetKeyboardLayout(0));

        */

        public uint GenLParams(int x, int y)
        {
            uint lParams = 0x000000000;
            lParams |= (uint)((y << 16) | (x & 0xFFFF));
            return lParams;
        }

        private void ClickerControlLoop()
        {
            IntPtr currentForegroundWindow = NativeMethods.GetForegroundWindow();
            IntPtr procWindow = NativeMethods.FindWindow(null, ProcessWindowTitle);
            Rect winRectangle = new Rect();
            bool gotRectangle = NativeMethods.GetWindowRect(procWindow, ref winRectangle);

            if (gotRectangle)
            {
                int x = (winRectangle.Right - winRectangle.Left) / 2;
                int y = (winRectangle.Bottom - winRectangle.Top) / 2;
                while (ClickerRunning)
                {
                    //NativeMethods.SetFocus(procWindow);
                    NativeMethods.PostMessage(procWindow, 0x200, 0, GenLParams(x, y));
                    NativeMethods.PostMessage(procWindow, 0x201, 0x1, GenLParams(x, y));
                    Thread.Sleep(10);
                    NativeMethods.PostMessage(procWindow, 0x200, 0, GenLParams(x, y));
                    NativeMethods.PostMessage(procWindow, 0x202, 0, GenLParams(x, y));
                    //NativeMethods.SetFocus(currentForegroundWindow);
                    Thread.Sleep(ClickInterval);
                }
            }
        }

        // Methods
        public void StartClicker()
        {
            if (!ClickerRunning && CurrentProcess != null)
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
            if (!HoldDownRunning && CurrentProcess != null)
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
            return CurrentProcess != null;
        }

    }
}
