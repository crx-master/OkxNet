﻿using System;

namespace OkxNet.Objects.Core
{
    public class OkxSocketPingPong
    {
        public DateTime PingTime { get; set; }
        public DateTime PongTime { get; set; }
        public string PongMessage { get; set; }
        public TimeSpan Latency { get; set; }
    }
}
