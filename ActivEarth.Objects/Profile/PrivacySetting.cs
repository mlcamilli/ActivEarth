using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects.Profile
{
    public class PrivacySetting
    {

        #region ---------- Public Members ----------

        public ProfileVisibility ProfileVisibility
        { 
            get;
            set;
        }

        public bool Email
        {
            get;
            set;
        }

        public bool Gender
        {
            get;
            set;
        }

        public bool Age
        {
            get;
            set;
        }

        public bool Weight
        {
            get;
            set;
        }

        public bool Height
        {
            get;
            set;
        }

        public bool Location
        {
            get;
            set;
        }

        public bool Group
        {
            get;
            set;
        }

        public int ID
        {
            get;
            set;
        }

        public int UserID
        {
            get;
            set;
        }

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