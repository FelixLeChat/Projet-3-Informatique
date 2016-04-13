using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using FrontEnd.Core;
using FrontEnd.StateMachine.Core;

namespace FrontEnd
{
    public static class Program
    {
        private const int FrameBySeconds = 60;
        public static object Lock = new object();
        private static TimeSpan _lastTimeSpan;
        private static TimeSpan _elapsedTime;
        private static readonly Stopwatch _chrono = Stopwatch.StartNew();
        private static TimeSpan _wantedElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / FrameBySeconds);
        public static MainWindow MainWindow;

        [STAThread]
        private static void Main()
        {
            var app = new Application();
            MainWindow = new MainWindow();
            MainWindow.Show();

            // Peut être l'équivalent de Idle
            //CompositionTarget.Rendering += OnIdle;

            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromTicks(TimeSpan.TicksPerSecond / FrameBySeconds / 3), DispatcherPriority.Render, OnIdle, Application.Current.Dispatcher);
            StateManager.Instance.Start();
            app.Run();
        }

        public static void resetTemps()
        {
            TimeSpan currentTime = _chrono.Elapsed;
            _lastTimeSpan = currentTime;
        }


        public static void OnIdle(object sender, EventArgs e)
        {
            var gameIsRunning = StateManager.Instance.IsGameRunning();

            // TODO: What is the use of this function
            NativeFunction.Message message;
            do {

                var currentTime = _chrono.Elapsed;
                var currentElapsedTime = currentTime - _lastTimeSpan;
                _lastTimeSpan = currentTime;

                _elapsedTime += currentElapsedTime;

                if (_elapsedTime >= _wantedElapsedTime)
                {
                    lock (Lock)
                    {
                        StateManager.Instance.Run((double)_elapsedTime.Ticks / TimeSpan.TicksPerSecond);
                        _elapsedTime = TimeSpan.Zero;
                    }
                }
            } while (!NativeFunction.PeekMessage(out message, IntPtr.Zero, 0, 0, 0) && gameIsRunning);
        }
    }
}
