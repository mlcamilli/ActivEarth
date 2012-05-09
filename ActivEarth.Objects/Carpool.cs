using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects
{
    public class Carpool
    {
        /// <summary>
        /// Unique identifier for the carpool.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// ID of the user who submitted the carpool.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Location where the carpool starts.
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// Location where the carpool ends.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Time of day at which the carpool will start.
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Number of seats currently available in the carpool.
        /// </summary>
        public byte SeatsAvailable  { get; set; }

        /// <summary>
        /// Comments submitted by the creator regarding the carpool.
        /// </summary>
        public string Comments { get; set; }
    }
}