using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivEarth.Competition.Contests
{
    /// <summary>
    /// This class represents a table that is used to display
    /// the bracket rewards for a contest.
    /// </summary>
    public partial class ActivityPointsTable : System.Web.UI.UserControl
    {
        /// <summary>
        /// Sets the Reward values of each bracket.
        /// </summary>
        /// <param name="rewards">The rewards of each level.</param>
        public void SetRewardValues(List<float> rewards)
        {
            if (rewards.Count == 6)
            {
                DiamondBracketReward.Text = rewards[0].ToString();
                PlatinumBracketReward.Text = rewards[1].ToString();
                GoldBracketReward.Text = rewards[2].ToString();
                SilverBracketReward.Text = rewards[3].ToString();
                BronzeBracketReward.Text = rewards[4].ToString();
                NoneBracketReward.Text = rewards[5].ToString();
            }
        }
    }
}