using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using FrontEnd.AsyncLoading;
using FrontEnd.Core;
using FrontEnd.Core.Event;
using FrontEnd.Player;
using FrontEnd.ProfileHelper;
using Newtonsoft.Json.Bson;
using EventManager = FrontEnd.Core.EventManager;
using Timer = System.Timers.Timer;

namespace FrontEnd.Helper
{
    /// <summary>
    /// Can detect if connected to Internet and can disconnect the player 
    /// </summary>
    public static class ConnectionHelper
    {
        public static int _errorCount = 0;
        public const int ErrourCountLimit = 5;


        // Object to synchronize if the application is connected to the network
        private static readonly object IsConnectedLock = new object();
        private static bool _isConnected;
        public static bool IsConnected {
            get
            {
                lock (IsConnectedLock)
                {
                    return _isConnected;
                }
            }
            set
            {
                lock (IsConnectedLock)
                {
                    _isConnected = value;
                }
            }
        }

        private static readonly object DisconnectLock = new object();
        private static bool _disconnectionWasHandle;

        private const double Interval = 500;
        private static Timer _connectionChecker;

        public static void StartCheckConnectionThread()
        {
            Interlocked.Exchange(ref _errorCount, 0);
            Debug.Assert(_connectionChecker == null, "ConnectionChecker should be set to null before reconnecting");
            _connectionChecker = new Timer(Interval);
            _connectionChecker.AutoReset = true;
            _connectionChecker.Elapsed += (sender, e) => { ConnectionCheckerTask(); };
            _connectionChecker.Start();
            IsConnected = true;
            _disconnectionWasHandle = false;
        }

        public static void StopCheckConnectionThread()
        {
            if (_connectionChecker != null)
            {
                _connectionChecker.Stop();
                _connectionChecker = null;
            }
        }

        private static void ConnectionCheckerTask()
        {
            var isConnected = IsConnectedToInterner();
            if (!isConnected)
            {
                Interlocked.Increment(ref _errorCount);
                Console.WriteLine("**** Fails #{0} in connection checker *****", _errorCount);
                if (Interlocked.Equals(_errorCount, ErrourCountLimit))
                {
                    Console.WriteLine("******************** Disconnection noticed *************************************");
                    IsConnected = false;
                    HandleDisconnection();
                }
            }
            else
            {
                Interlocked.Exchange(ref _errorCount, 0);
            }
        }


        public static bool IsConnectedToInterner()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Will disconnect the user 
        /// </summary>
        public static void HandleDisconnection()
        {
            if (Monitor.TryEnter(DisconnectLock))
            {
                StopCheckConnectionThread();
                if (_disconnectionWasHandle)
                {
                    return;
                }
                try
                {
                    Load.Disconnect();
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        
                        MessageHelper.ShowMessage("Déconnecté des internets",
                            "Votre messager et son beau chapeau ne semble pas revenir.");
                    }));
                    _disconnectionWasHandle = true;
                }
                finally
                {
                    Monitor.Exit(DisconnectLock);
                }
            }
        }
    }
}