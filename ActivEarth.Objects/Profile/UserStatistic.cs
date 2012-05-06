using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ActivEarth.Objects.Profile
{
    [DataContract]
    public class UserStatistic
    {
        #region ---------- Public Members ----------

        [DataMember]
        public int UserStatisticID
        {
            get;
            set;
        }

        [DataMember]
        public int UserID
        {
            get;
            set;
        }

        [DataMember]
        public Statistic Statistic
        {
            get;
            set;
        }

        [DataMember]
        public float Value
        {
            get;
            set;
        }
        
        #endregion ---------- Public Members ----------

        #region ---------- Constructor ----------

        public UserStatistic(Statistic stat)
            : this(stat, 0)
        {

        }

        public UserStatistic(Statistic stat, float val)
        {
            Statistic = stat;
            Value = val;
        }

        #endregion ---------- Constructor ----------

    }
}