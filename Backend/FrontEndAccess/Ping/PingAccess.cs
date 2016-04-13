using System;
using System.Diagnostics;
using System.Net;
using Helper.Http;

namespace FrontEndAccess.Ping
{
    public class PingAccess
    {
        public static PingAccess Instance;
        private readonly string _endpoint;

        // Set endpoint to spedified value
        public PingAccess(string endpoint)
        {
            Instance = this;
            _endpoint = endpoint + "/api/ping";
        }

        public double GetPingMs()
        {
            var stopWatch = Stopwatch.StartNew();
            var httpResponse = HttpRequestHelper.GetAsync(_endpoint).Result;
            stopWatch.Stop();
            return stopWatch.Elapsed.TotalMilliseconds;
        }
    }
}
