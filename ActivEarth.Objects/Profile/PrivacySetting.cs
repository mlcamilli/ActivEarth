using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects.Profile
{
    public class PrivacySetting
    {

        #region ---------- Public Members ----------

        /// <summary>
        /// Field representing the overall visibility of a user's profile.
        /// </summary>
        public ProfileVisibility ProfileVisibility
        { 
            get;
            set;
        }

        /// <summary>
        /// True if the user's email is visible, false otherwise.
        /// </summary>
        public bool Email
        {
            get;
            set;
        }

        /// <summary>
        /// True if the user's gender is visible, false otherwise.
        /// </summary>
        public bool Gender
        {
            get;
            set;
        }

        /// <summary>
        /// True if the user's age is visible, false otherwise.
        /// </summary>
        public bool Age
        {
            get;
            set;
        }

        /// <summary>
        /// True if the user's weight is visible, false otherwise.
        /// </summary>
        public bool Weight
        {
            get;
            set;
        }

        /// <summary>
        /// True if the user's height visible, false otherwise.
        /// </summary>
        public bool Height
        {
            get;
            set;
        }

        /// <summary>
        /// True if the user's location is visible, false otherwise.
        /// </summary>
        public bool Location
        {
            get;
            set;
        }

        /// <summary>
        /// True if the user's groups are visible, false otherwise.
        /// </summary>
        public bool Group
        {
            get;
            set;
        }

        /// <summary>
        /// A unique identification number for the privacy setting.
        /// </summary>
        public int ID
        {
            get;
            set;
        }

        /// <summary>
        /// A unique identification number for the privacy setting's user.
        /// </summary>
        public int UserID
        {
            get;
            set;
        }

        /// <summary>
        /// The user owning the privacy setting.
        /// </summary>
        public User User
        {
            get;
            set;
        }

        #endregion ---------- Public Members ----------

        #region ---------- Constructor ----------

        public PrivacySetting()
            : this(bool.FalseString, bool.TrueString, bool.TrueString, bool.TrueString, bool.TrueString, bool.TrueString, bool.TrueString)
        {

        }

        public PrivacySetting(bool email, bool gender, bool age, bool weight, bool height, bool location, bool group)
        {
            this.Email = email;
            this.Gender = gender;
            this.Age = age;
            this.Weight = weight;
            this.Height = height;
            this.Location = location;
            this.Group = group;
        }

        public PrivacySetting(string email, string gender, string age, string weight, string height, string location, string group)
        {
            this.Email = bool.Parse(email);
            this.Gender = bool.Parse(gender);
            this.Age = bool.Parse(age);
            this.Weight = bool.Parse(weight);
            this.Height = bool.Parse(height);
            this.Location = bool.Parse(location);
            this.Group = bool.Parse(group);
        }

        #endregion ---------- Constructor ----------

    }
}