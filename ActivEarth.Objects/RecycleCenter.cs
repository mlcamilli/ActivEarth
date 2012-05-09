using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects
{
    public class RecycleCenter
    {
        /// <summary>
        /// Unique identifier for the Recycling Center.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// ID of the user who submitted the Recycling Center.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Location of the Recycling Center.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Comments from the submitter regarding the Recycling Center.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// True if the center accepts Automotive items, false otherwise.
        /// </summary>
        public bool Automotive { get; set; }

        /// <summary>
        /// True if the center accepts Electronic items, false otherwise.
        /// </summary>
        public bool Electronics { get; set; }

        /// <summary>
        /// True if the center accepts Contruction items, false otherwise.
        /// </summary>
        public bool Construction { get; set; }

        /// <summary>
        /// True if the center accepts Batteries, false otherwise.
        /// </summary>
        public bool Batteries { get; set; }

        /// <summary>
        /// True if the center accepts Garden items, false otherwise.
        /// </summary>
        public bool Garden { get; set; }

        /// <summary>
        /// True if the center accepts Glass items, false otherwise.
        /// </summary>
        public bool Glass { get; set; }

        /// <summary>
        /// True if the center accepts Hazardous items, false otherwise.
        /// </summary>
        public bool Hazardous { get; set; }

        /// <summary>
        /// True if the center accepts Household items, false otherwise.
        /// </summary>
        public bool Household { get; set; }

        /// <summary>
        /// True if the center accepts Metal items, false otherwise.
        /// </summary>
        public bool Metal { get; set; }

        /// <summary>
        /// True if the center accepts Paint, false otherwise.
        /// </summary>
        public bool Paint { get; set; }

        /// <summary>
        /// True if the center accepts Paper items, false otherwise.
        /// </summary>
        public bool Paper { get; set; }

        /// <summary>
        /// True if the center accepts Plastic items, false otherwise.
        /// </summary>
        public bool Plastic { get; set; }
    }
}