using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition;


namespace ActivEarth.Objects.Groups
{
    public class RecentActivity : Message
    {
        /// <summary>
        /// The amount that should be added to the green score due to the activity.
        /// </summary>
        uint AddGreenScore
        {
            get;
            set;
        }

        /// <summary>
        /// The amount that should be added to the activity score due to the activity.
        /// </summary>
        ActivityScore AddActivityScore
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a Recent Activity object representing a Wall Message that resulted from 
        /// a User's recent activity.
        /// </summary>
        /// <param name="addactivityscore">The amount to add to the activityscore as a result of the activity</param>
        /// <param name="addgreenscore">The amount to add to the greenscore as a result of the activity</param>
        /// <param name="title">The title of the Message</param>
        /// <param name="text">The text in the Message</param>
        /// <param name="poster">The User who posted the Message</param>
        public RecentActivity(ActivityScore addactivityscore, uint addgreenscore, string title, string text, User poster) : base(title, text, poster)
        {
            this.AddActivityScore = addactivityscore;
            this.AddGreenScore = addgreenscore;
        }
    }
}