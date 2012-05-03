using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Objects.Profile;
using ActivEarth.DAO;
using ActivEarth.Server.Service.Competition;
using ActivEarth.Objects.Groups;

namespace ActivEarth.Competition.Contests
{
    /// <summary>
    /// This class represents the display Contest page in the ActivEarth
    /// website.
    /// </summary>
    public partial class DisplayContestPage : System.Web.UI.Page
    {
        int contestId;
        User user;
        bool isValidId;
        bool isGroup;

        /// <summary>
        /// Loads the page.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The Event Args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                string contestIdString = Request.QueryString["id"];
                
                Contest contest = null;
                if(contestIdString != null && int.TryParse(contestIdString, out contestId))
                {
                    contest = ContestDAO.GetContestFromContestId(contestId, false, false); 
                }

                isValidId = contest != null;
                if (isValidId)
                {

#region Test Code ------------------------------------------------------------------------

ContestDAO.UpdateContestStandings(contestId);

#endregion Test Code ------------------------------------------------------------------------

                    isGroup = contest.Type == ContestType.Group;
                    if (!Page.IsPostBack)
                    {
                        LoadDataOnPage();
                    }
                }
            }
        }

        /// <summary>
        /// Loads the content on the page.
        /// </summary>
        private void LoadDataOnPage()
        {
            Contest contest = ContestDAO.GetContestFromContestId(contestId, true, false);
            string contestState = contest.getContestState();
            bool isCompeting = TeamDAO.UserCompetingInContest(user.UserID, contestId);

            ContestName.Text = contest.Name;
            ContestDescription.Text = contest.Description;

            if (contestState == "SIGN-UP")
            {
                LoadContestSignupData(contest, isCompeting);
            }
            else 
            {
                ContestStatsTable.Visible = true;
                TotalTeamsLabel.Text = contest.Teams.Count.ToString();
                TotalRewardLabel.Text = contest.Reward.ToString();

                if (contest.Mode == ContestEndMode.TimeBased)
                {
                    ProgressGraph.PopulateProgressGraph(contest.StartTime, contest.EndCondition.EndTime);
                }
                else
                {
                    ProgressGraph.PopulateProgressGraph(
                        (contest.Teams.Count >= 1 ? contest.Teams[0] : null),
                        contest.EndCondition.EndValue,
                        contest.FormatString);
                }

                if (isCompeting)
                {
                    Team usersTeam = TeamDAO.GetTeamFromUserIdAndContestId(user.UserID, contest.ID, false);
                    List<Team> teamsToDisplay = ContestManager.GetTeamsToDisplay(usersTeam, contest);
                    Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                    Color[] textColors = { Color.White, Color.Black };
                    List<int> rewards = ContestDAO.CalculateBracketRewards(contest);
                    ContestLeaderBoard.PopulateLeaderBoard(teamsToDisplay, backColors, textColors, contest.FormatString, rewards);
                    
                    ContestLeaderBoard.Visible = true;
                    ContestSignUpPanel.Visible = false;
                }
                else
                {
                    ContestSignUpPanel.Visible = true;
                    if (contest.Type == ContestType.Group)
                    {
                        if (GroupSelection.Items.Count == 0)
                        {
                            List<Group> groups = GroupDAO.GetAllGroupsByOwner(user);

                            foreach (Group group in groups)
                            {
                                GroupSelection.Items.Add(group.Name);
                            }

                            if (groups.Count != 0)
                            {
                                btnJoinContest.Visible = true;
                                GroupSelection.Visible = true;
                            }
                            else
                            {
                                SignUpErrorMessage.Text = "This is a group contest. In order to join a group contest, you must "
                                    + "be a group leader of at least one group. If you are part of a group and not the leader, ask your group leader to add "
                                    + "the group to this contest.";
                                SignUpErrorMessage.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        btnJoinContest.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Loads content specific to contest Signup.
        /// </summary>
        /// <param name="contest">The contest.</param>
        /// <param name="isCompeting">Whether the team is currently competeing or not.</param>
        private void LoadContestSignupData(Contest contest, bool isCompeting)
        {
            if (contest.Type == ContestType.Group)
            {
                if (!isCompeting)
                {
                    btnLeaveContest.Visible = false;

                    if (GroupSelection.Items.Count == 0)
                    {
                        List<Group> groups = GroupDAO.GetAllGroupsByOwner(user);

                        foreach (Group group in groups)
                        {
                            GroupSelection.Items.Add(group.Name);
                        }
                    }

                    if (GroupSelection.Items.Count != 0)
                    {
                        btnJoinContest.Visible = true;
                        GroupSelection.Visible = true;
                    }
                    else
                    {
                        SignUpErrorMessage.Text = "This is a group contest. In order to join a group contest, you must "
                            + "be a group leader of at least one group. If you are part of a group and not the leader, ask your group leader to add "
                            + "the group to this contest.";
                        SignUpErrorMessage.Visible = true;
                    }
                }
                else
                {
                    if (contest.Type == ContestType.Group)
                    {
                        Team team = TeamDAO.GetTeamFromUserIdAndContestId(user.UserID, contestId, true);
                        Group group = GroupDAO.GetGroupFromGroupId(team.GroupId ?? -1);
                        if (group.Owner.UserID == user.UserID)
                        {
                            btnLeaveContest.Visible = true;
                        }
                        else
                        {
                            btnLeaveContest.Visible = false;
                        }
                    }
                    else
                    {
                        btnLeaveContest.Visible = true;
                    }

                    btnJoinContest.Visible = false;
                    GroupSelection.Visible = false;
                }
            }
            else
            {
                btnJoinContest.Visible = !isCompeting;
                btnLeaveContest.Visible = isCompeting;
            }

            ContestSignUpPanel.Visible = true;
            Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
            Color[] textColors = { Color.White, Color.Black };

            if (contest.Teams.Count == 0)
            {
                CurrentTeamsSignedUp.Visible = false;
                if (contest.Type == ContestType.Group)
                {
                    NoTeamsMessage.Text = "No groups have signed up for this contest.";
                }
                else
                {
                    NoTeamsMessage.Text = "No one has signed up for this contest.";
                }

                NoTeamsMessage.Visible = true;
            }
            else
            {
                CurrentTeamsSignedUp.Visible = true;
                NoTeamsMessage.Visible = false;
                CurrentTeamsSignedUp.PopulateTeamTable(contest.Teams, backColors, textColors);
            }
        }

        /// <summary>
        /// Adds a team to the contest.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event args.</param>
        protected void JoinContest(object sender, EventArgs e)
        {
            if(isValidId)
            {
                if (isGroup)
                {
                    Group group = GroupDAO.GetAllGroupsByOwner(user)[GroupSelection.SelectedIndex];
                    ContestManager.AddGroup(contestId, group);
                }
                else
                {
                    ContestManager.AddUser(contestId, user);
                }
                
                LoadDataOnPage();
            }
        }

        /// <summary>
        /// Removes a team from a contest.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event args.</param>
        protected void LeaveContest(object sender, EventArgs e)
        {
            if (isValidId)
            {
                if (isGroup)
                {
                    Team team = TeamDAO.GetTeamFromUserIdAndContestId(user.UserID, contestId, true);
                    if (team != null)
                    {
                        ContestManager.RemoveGroup(contestId, GroupDAO.GetGroupFromGroupId(team.GroupId ?? -1));
                    }
                }
                else
                {
                    ContestManager.RemoveUser(contestId, user);
                }

                LoadDataOnPage();
            }
        }
    }
}