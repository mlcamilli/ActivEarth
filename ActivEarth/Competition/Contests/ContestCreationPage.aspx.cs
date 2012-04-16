using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivEarth.Competition.Contests
{
    public partial class ContestCreationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            contestStartDateCalender.StartDate = DateTime.Now;
        }

        protected void ddlContestMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string mode = ddlContestMode.SelectedValue;
            if (mode == "Time")
            {
                contestModeTimePanel.Visible = true;
                contestModeGoalPanel.Visible = false;
            }
            else
            {
                contestModeGoalPanel.Visible = true;
                contestModeTimePanel.Visible = false;
            }
        }

        protected void CreateContest(object sender, EventArgs e)
        {

        }
    }
}