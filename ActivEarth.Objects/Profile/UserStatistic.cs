﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects.Profile
{
    public class UserStatistic
    {
        #region ---------- Public Members ----------

        public int UserStatisticID
        {
            get;
            set;
        }

        public int UserID
        {
            get;
            set;
        }

        public Statistic statistic
        {
            get;
            set;
        }

        public float value
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
            statistic = stat;
            value = val;
        }

        #endregion ---------- Constructor ----------

    }
}