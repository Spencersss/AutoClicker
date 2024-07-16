using System;
using System.Diagnostics;
using System.Threading;
using SpencerAutoClicker.Source.Model.Natives;

namespace SpencerAutoClicker.Source.Model
{
    public class Clicker
    {
        // States
        public bool ClickerRunning { get; set; } // left click running
        private Process CurrentProcess { get; set; } // current process clicker interacts with
        private Thread ControlLoopThread;

        // Properties
        public string ProcessWindowTitle => CurrentProcess.MainWindowTitle;

        // Constants
        private const int MouseLeftDown = 0x201;
        private const int MouseLeftUp = 0x202;

        // Constructor
        public Clicker()
        {
            // Init state vals
            ClickerRunning = false;
            CurrentProcess = null;
            ControlLoopThread = null;
        }

        // Methods
        public void SetProcess(Process proc)
        {
            CurrentProcess = proc;
        }

        public Process GetProcess()
        {
            return CurrentProcess;
        }

        public bool IsProcessSelected()
        {
            return CurrentProcess != null;
        }

        /*
         
        Getting scan codes of left mouse button up/down
          
        uint mouseLeftDown = NativeMethods.MapVirtualKeyEx(
            (uint)MouseButton.LeftDown, 0, NativeMethods.GetKeyboardLayout(0));
        uint mouseLeftUp = NativeMethods.MapVirtualKeyEx(
            (uint)MouseButton.LeftUp, 0, NativeMethods.GetKeyboardLayout(0));

        */
        private uint GenLParams(int x, int y)
        {
            uint lParams = 0x000000000;
            lParams |= (uint)(y << 16 | x & 0xFFFF);
            return lParams;
        }

        private void ClickerControlLoop()
        {
            IntPtr procWindow = NativeMethods.FindWindow(null, ProcessWindowTitle);
            Natives.Rect winRectangle = new Natives.Rect();
            bool gotRectangle = NativeMethods.GetWindowRect(procWindow, ref winRectangle);

            if (gotRectangle)
            {
                int x = (winRectangle.Right - winRectangle.Left) / 2;
                int y = (winRectangle.Bottom - winRectangle.Top) / 2;

                // When clicker starts and hold down left mode is enabled, send single mouse down to process
                if (ClickerSettings.ShouldHoldDown)
                {
                    NativeMethods.PostMessage(procWindow, MouseLeftDown, 0x1, GenLParams(x, y));
                }

                while (ClickerRunning)
                {
                    if (!ClickerSettings.ShouldHoldDown)
                    {
                        NativeMethods.PostMessage(procWindow, MouseLeftDown, 0x1, GenLParams(x, y));
                        Thread.Sleep(10);
                        NativeMethods.PostMessage(procWindow, MouseLeftUp, 0, GenLParams(x, y));
                        Thread.Sleep(ClickerSettings.ClickInterval);
                    }
                }

                // When clicker stops, send mouse up input if being held down
                if (ClickerSettings.ShouldHoldDown)
                {
                    NativeMethods.PostMessage(procWindow, MouseLeftUp, 0, GenLParams(x, y));
                }
            }
        }

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

    }
}
