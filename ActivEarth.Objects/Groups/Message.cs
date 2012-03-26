using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ActivEarth.Objects.Profile;

namespace ActivEarth.Objects.Groups
{
    public class Message
    {
        /// <summary>
        /// The title of the Message.
        /// </summary>

        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// The text that should be displayed in the message.
        /// </summary>
        /// 
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

        /// <summary>
        /// Creates a Message to be posted on a Group or User's Wall.
        /// </summary>
        /// <param name="title">The title of the Message</param>
        /// <param name="text">The text in the Message</param>
        /// <param name="poster">The User who posted the Message</param>
        public Message(string title, string text, User poster)
        {
            this.Title = title;
            this.Text = text;
            this.Poster = poster;
        }
    }
}