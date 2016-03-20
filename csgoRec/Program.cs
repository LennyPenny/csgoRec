using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using CSGSI;
using CSGSI.Nodes;

namespace csgoRec
{
    class Program
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_MINIMIZE = 6;


        private static GameStateListener gsl;
        private static bool recording = false;

        static void Main(string[] args)
        {
            gsl = new GameStateListener(3000);

            gsl.NewGameState += gs =>
            {
                if (!recording && gs.Player.Activity == PlayerActivity.Playing)
                    Record();
                else if (recording && gs.Player.Activity == PlayerActivity.Menu)
                    StopRecording();
            };

            if (!gsl.Start()) Environment.Exit(0);
            Console.WriteLine("Listening...");

            IntPtr winHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(winHandle, SW_MINIMIZE);

        }

        private static void StopRecording()
        {
            InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);
            InputSimulator.SimulateKeyDown(VirtualKeyCode.F9);

            recording = false;
            Thread.Sleep(100);

            InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F9);

            Console.WriteLine("Stopped recording...");
        }

        private static void Record()
        {
            InputSimulator.SimulateKeyDown(VirtualKeyCode.MENU);
            InputSimulator.SimulateKeyDown(VirtualKeyCode.F9);

            recording = true;

            Thread.Sleep(100);

            InputSimulator.SimulateKeyUp(VirtualKeyCode.MENU);
            InputSimulator.SimulateKeyUp(VirtualKeyCode.F9);

            Console.WriteLine("Recording...");
        }
    }
}
