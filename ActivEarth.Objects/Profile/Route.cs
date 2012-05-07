using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects.Profile
{
    public class Route
    {
        public int UserId { get; set; }

        public int GMTOffset { get; set; }

        public double Distance { get; set; }

        public double EndLatitude { get; set; }

        public double EndLongitude { get; set; }

        public DateTime EndTime { get; set; }

        public string Mode { get; set; }

        public string Points { get; set; }

        public double StartLatitude { get; set; }

        public double StartLongitude { get; set; }

        public DateTime StartTime { get; set; }

        public int Steps { get; set; }

        public string Type { get; set; }
    }
}