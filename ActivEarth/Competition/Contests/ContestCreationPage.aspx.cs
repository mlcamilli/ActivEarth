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
    /// <summary>
    /// This class represents the Contest Creation Page on
    /// the ActivEarth website.
    /// </summary>
    public partial class ContestCreationPage : System.Web.UI.Page
    {
        User user;

        /// <summary>
        /// Loads the page.
        /// </summary>
        /// <param name="sender">Object that requested the page load.</param>
        /// <param name="e">The event arguments.</param>
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

        /// <summary>
        /// Index changed event for contest mode selection. 
        /// Used to update the user fields when the contest mode
        /// is changed.
        /// </summary>
        /// <param name="sender">Object that sent the event</param>
        /// <param name="e">The event arguments.</param>
        protected void ddlContestMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetModeDisplay();
        }

        /// <summary>
        /// Used to validate that the start date of a contest is after
        /// the current date.
        /// </summary>
        /// <param name="sender">Object that sent the event</param>
        /// <param name="e">The event arguments.</param>
        protected void ValidateStartDate(object source, ServerValidateEventArgs args)
        {
            DateTime startDate = DateTime.Parse(args.Value);
            args.IsValid = (DateTime.Now < startDate);
        }

        /// <summary>
        /// Used to validate that the end date of a contest is on or after
        /// the start date.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void ValidateEndDate(object source, ServerValidateEventArgs args)
        {
            DateTime startDate = DateTime.Parse(txbContestStartDate.Text);
            DateTime endDate = DateTime.Parse(args.Value);
            args.IsValid = (startDate <= endDate);
        }

        /// <summary>
        /// Click event for the Create Contest button. Creates a Contest
        /// from the user supplied fields.
        /// </summary>
        /// <param name="sender">Object that sent the event</param>
        /// <param name="e">The event arguments.</param>
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

                    createdContestId = ContestManager.CreateContest(
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

                Response.Redirect("~/Competition/Contests/DisplayContestPage.aspx?id=" + createdContestId);
            }
        }

        /// <summary>
        /// Updates the display on the page to display the correct
        /// Contest end mode entry.
        /// </summary>
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