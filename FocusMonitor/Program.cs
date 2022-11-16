using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Timers;

namespace FocusMonitor
{
    class Program
    {
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        static void Main(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += GetActiveWindow;
            timer.AutoReset = true;
            timer.Enabled = true;
            Console.ReadLine();
        }

        private static string _lastFocusedWindowTitle = "";
        private static int _lastFocusedWindowHandle;

        private static void GetActiveWindow(object o, ElapsedEventArgs arg)
        {
            const int numberOfChars = 256;
            StringBuilder sb = new StringBuilder(numberOfChars);
            int handle = GetForegroundWindow();
            if (GetWindowText(handle, sb, numberOfChars) > 0)
            {
                if (handle != _lastFocusedWindowHandle || sb.ToString() != _lastFocusedWindowTitle)
                {
                    _lastFocusedWindowHandle = handle;
                    _lastFocusedWindowTitle = sb.ToString();
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} ({handle}) {sb}");
                }
            }
            else
            {
                if (handle != _lastFocusedWindowHandle || sb.ToString() != _lastFocusedWindowTitle)
                {
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} (none)");
                    _lastFocusedWindowHandle = handle;
                    _lastFocusedWindowTitle = "";
                }
            }
        }
    }
}
