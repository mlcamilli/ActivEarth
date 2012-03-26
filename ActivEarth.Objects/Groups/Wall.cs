using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects.Groups
{
    public class Wall
    {
        /// <summary>
        /// The Maximum number of messages that should be displayed on the Wall.
        /// </summary>
        int MAX_MESSAGES = 50;

        /// <summary>
        /// The List of Messages to display on the Wall.
        /// </summary>
        public List<Message> Messages
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new Wall object where Messages can be posted.
        /// </summary>
        public Wall()
        {
            this.Messages = new List<Message>(MAX_MESSAGES);
        }

        /// <summary>
        /// Adds a Message to the Wall, removing older Messages in FIFO order when
        /// the capacity is exceeded.
        /// </summary>
        /// <param name="message"></param>
        public void post(Message message)
        {
            if (Messages.Count == MAX_MESSAGES)
            {
                Messages.RemoveAt(0);
            }

            Messages.Add(message);
        }
    }
}