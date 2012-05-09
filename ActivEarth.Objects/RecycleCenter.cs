using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects
{
    public class RecycleCenter
    {
        public int ID { get; set; }

        public int UserId { get; set; }

        public string Location { get; set; }

        public string Comments { get; set; }

        public bool Automotive { get; set; }

        public bool Electronics { get; set; }

        public bool Construction { get; set; }

        public bool Batteries { get; set; }

        public bool Garden { get; set; }

        public bool Glass { get; set; }

        public bool Hazardous { get; set; }

        public bool Household { get; set; }

        public bool Metal { get; set; }

        public bool Paint { get; set; }

        public bool Paper { get; set; }

        public bool Plastic { get; set; }
    }
}