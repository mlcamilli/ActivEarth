using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service.Competition;
using ActivEarth.DAO;

namespace ActivEarth.Competition.Contests
{
    public partial class ContestCreationPage : System.Web.UI.Page
    {
        User user;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                contestStartDateCalender.StartDate = DateTime.Today.AddDays(1);

                int i = 0;
                string statName;

                if (ddlStatisticMeasured.Items.Count == 0)
                {
                    while ((statName = StatisticInfoDAO.GetStatisticName((Statistic)i)) != String.Empty)
                    {
                        ddlStatisticMeasured.Items.Add(statName);
                        i++;
                    }
                }
            }
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
            // ---------------------------------------
            // ---------------------------------------
            // ----- Still needs data validation -----
            // ---------------------------------------
            // ---------------------------------------
         
            Statistic statistic = (Statistic)(ddlStatisticMeasured.SelectedIndex);

            if (ddlContestMode.SelectedIndex == (int)ContestEndMode.GoalBased)
            {
                DateTime startDate = DateTime.Parse(txbContestStartDate.Text);
                float endGoal = float.Parse(txbContestEndGoal.Text);

                ContestManager.CreateContest(
                    (ddlContestType.SelectedValue.Equals("Group") ? ContestType.Group : ContestType.Individual),
                    txbContestName.Text,
                    txbContestDescription.Text,
                    ContestManager.CalculateContestReward(float.Parse(txbContestEndGoal.Text), statistic),
                    startDate,
                    endGoal,
                    chkContestSearchable.Checked,
                    statistic,
                    user.UserID);
            }
            else
            {
                DateTime startDate = DateTime.Parse(txbContestStartDate.Text);
                DateTime endDate = DateTime.Parse(txbContestEndDate.Text);

                TimeSpan length = endDate.Subtract(startDate);

                ContestManager.CreateContest(
                    (ddlContestType.SelectedValue.Equals("Group") ? ContestType.Group : ContestType.Individual),
                    txbContestName.Text,
                    txbContestDescription.Text,
                    ContestManager.CalculateContestReward(length, statistic),
                    startDate,
                    endDate,
                    chkContestSearchable.Checked,
                    statistic,
                    user.UserID);
            }

            Response.Redirect("~/Competition/Contests/ContestHomePage.aspx");
        }
    }
}