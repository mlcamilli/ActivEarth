using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivEarth.Competition.Contests
{
    /// <summary>
    /// This class represents a control used to display the time remaining 
    /// in a contest.
    /// </summary>
    public partial class ContestTimeGraph : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets the progress time has made in the contest.
        /// </summary>
        /// <param name="contestStartDate">The start date of the contest.</param>
        /// <param name="contestEndDate">The end date of the contest.</param>
        public void PopulateTimeGraph(DateTime contestStartDate, DateTime contestEndDate)
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
    }
}