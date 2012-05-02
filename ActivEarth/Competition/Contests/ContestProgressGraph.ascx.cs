using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Competition.Contests
{
    /// <summary>
    /// This class represents a control used to display the time remaining 
    /// in a contest.
    /// </summary>
    public partial class ContestTimeGraph : System.Web.UI.UserControl
    {
        /// <summary>
        /// Sets the progress time has made in the contest.
        /// </summary>
        /// <param name="contestStartDate">The start date of the contest.</param>
        /// <param name="contestEndDate">The end date of the contest.</param>
        public void PopulateProgressGraph(DateTime contestStartDate, DateTime contestEndDate)
        {
            if(DateTime.Compare(contestEndDate, DateTime.Now) > 0)
            {
                TimeSpan contestSpan = contestEndDate - contestStartDate; 
                TimeSpan timeCompleted = contestEndDate - DateTime.Now;

                _ContestProgress.Value = (int)(((contestSpan.TotalDays - timeCompleted.TotalDays) / contestSpan.TotalDays) * 100);
            }
            else
            {
                _ContestProgress.Value = 100;
            }
        }

        /// <summary>
        /// Sets the prgress towards the goal the top contributor has made.
        /// </summary>
        /// <param name="firstPlace">The team in first.</param>
        /// <param name="goal">The goal of the contest.</param>
        /// <param name="format">The format string the goal value is.</param>
        public void PopulateProgressGraph(Team firstPlace, float goal, string format)
        {
            _ContestProgress.Value = (firstPlace != null ? (int)((firstPlace.Score / goal) * 100) : 0);
            _StartLabel.Text = 0.ToString(format);
            _EndLabel.Text = goal.ToString(format);
        }
    }
}