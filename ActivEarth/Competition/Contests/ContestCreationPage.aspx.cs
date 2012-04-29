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

                if (txbContestStartDate.Text.Length == 0)
                {
                    contestStartDateCalender.SelectedDate = DateTime.Today.AddDays(1);
                }

                if (txbContestEndDate.Text.Length == 0)
                {
                    contestEndDateCalender.SelectedDate = contestStartDateCalender.SelectedDate;
                }

                SetModeDisplay();
            }
        }

        protected void ddlContestMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetModeDisplay();
        }

        protected void ValidateStartDate(object source, ServerValidateEventArgs args)
        {
            DateTime startDate = DateTime.Parse(args.Value);
            args.IsValid = (DateTime.Now < startDate);
        }

        protected void ValidateEndDate(object source, ServerValidateEventArgs args)
        {
            DateTime startDate = DateTime.Parse(txbContestStartDate.Text);
            DateTime endDate = DateTime.Parse(args.Value);
            args.IsValid = (startDate <= endDate);
        }

        protected void CreateContest(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                Statistic statistic = (Statistic)(ddlStatisticMeasured.SelectedIndex);
                int createdContestId;
                if (ddlContestMode.SelectedIndex == (int)ContestEndMode.GoalBased)
                {
                    DateTime startDate = DateTime.Parse(txbContestStartDate.Text);
                    txbContestStartDate.Text = "" + startDate.Day;
                    float endGoal = float.Parse(txbContestEndGoal.Text);

                    createdContestId = ContestManager.CreateContest(
                        (ddlContestType.SelectedValue.Equals("Group") ? ContestType.Group : ContestType.Individual),
                        txbContestName.Text,
                        txbContestDescription.Text,
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

                    createdContestId = ContestManager.CreateContest(
                        (ddlContestType.SelectedValue.Equals("Group") ? ContestType.Group : ContestType.Individual),
                        txbContestName.Text,
                        txbContestDescription.Text,
                        startDate,
                        endDate,
                        chkContestSearchable.Checked,
                        statistic,
                        user.UserID);
                }

                Response.Redirect("~/Competition/Contests/DisplayContestPage.aspx?id=" + createdContestId);
            }
        }

        private void SetModeDisplay()
        {
            string mode = ddlContestMode.SelectedValue;
            if (mode == "Time")
            {
                contestEndDateLabel.Visible = true;
                contestEndGoalLabel.Visible = false;
                contestModeTimePanel.Visible = true;
                contestModeGoalPanel.Visible = false;
            }
            else
            {
                contestEndDateLabel.Visible = false;
                contestEndGoalLabel.Visible = true;
                contestModeGoalPanel.Visible = true;
                contestModeTimePanel.Visible = false;
            }
        }
    }
}