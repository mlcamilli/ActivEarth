using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects
{
    public class Carpool
    {
        public int ID { get; set; }

        public int UserId { get; set; }

        public string Start { get; set; }

        public string Destination { get; set; }

        public string Time { get; set; }

        public byte SeatsAvailable  { get; set; }

        public string Comments { get; set; }
    }
}