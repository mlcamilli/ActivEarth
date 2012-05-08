using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using ActivEarth.Objects.Profile;

namespace ActivEarth.Objects.Groups
{
    [DataContract]
    public class Message
    {
        /// <summary>
        /// The title of the Message.
        /// </summary>
        [DataMember]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// The text that should be displayed in the message.
        /// </summary>
        /// 
        [DataMember]
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the User who posted the message.
        /// </summary>
        public User Poster
        {
            get;
            set;
        }

        public string PosterName { get; set; }

        /// <summary>
        /// The date that the message was posted.
        /// </summary>
        [DataMember]
        public string Date
        {
            get;
            set;
        }

        /// <summary>
        /// The time that the message was posted.
        /// </summary>
        [DataMember]
        public string Time
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a Message to be posted on a Group or User's Wall.
        /// </summary>
        /// <param name="title">The title of the Message</param>
        /// <param name="text">The text in the Message</param>
        /// <param name="poster">The User who posted the Message</param>
        public Message(string title, string text, User poster, string date, string time)
        {
            this.Title = title;
            this.Text = text;
            this.Poster = poster;
            this.Date = date;
            this.Time = time;
            this.PosterName = poster.UserName;
        }
    }
}