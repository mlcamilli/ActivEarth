using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivEarth.Competition.Contests
{
    public partial class ContestTimeGraph : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

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