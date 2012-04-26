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

namespace ActivEarth.Competition.Contests
{
    public partial class DisplayContestPage : System.Web.UI.Page
    {
        int contestId;
        User user;
        bool isValidId;

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
                
                Contest contest = contest = null;
                if(contestIdString != null && int.TryParse(contestIdString, out contestId))
                {
                    contest = ContestDAO.GetContestFromContestId(contestId, false, false); 
                }

                isValidId = contest != null;
                if (!Page.IsPostBack && isValidId)
                {
                    LoadDateOnPage();
                }
            }
        }

        private void LoadDateOnPage()
        {
            Contest contest = ContestDAO.GetContestFromContestId(contestId, true, false); 

            ContestName.Text = contest.Name;
            ContestDescription.Text = contest.Description;
            ContestActivityScore.Text = contest.Reward.ToString();

            if (contest.StartTime > DateTime.Now)
            {
                btnJoinContest.Visible =
                    ((contest.Type == ContestType.Individual) &&
                        !(TeamDAO.UserCompetingInContest(user.UserID, contestId)));

                ContestSignUpPanel.Visible = true;
                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };
                CurrentTeams.PopulateTeamTable(contest.Teams, backColors, textColors);
            }
            else
            {
                if (contest.Mode == ContestEndMode.TimeBased)
                {
                    TimeGraph.PopulateTimeGraph(contest.StartTime, contest.EndCondition.EndTime);
                    TimeGraph.Visible = true;
                }
                else
                {
                    GoalGraph.PopulateContestGraph(
                        (contest.Teams.Count >= 1 ? contest.Teams[0] : null),
                        (contest.Teams.Count >= 2 ? contest.Teams[1] : null),
                        (contest.Teams.Count >= 3 ? contest.Teams[2] : null),
                        TeamDAO.GetTeamFromUserIdAndContestId(user.UserID, contestId, false),
                        contest.EndCondition.EndValue);

                    GoalGraph.SetGraphLabels(contest.EndCondition.EndValue, contest.FormatString);
                    GoalGraph.Visible = true;
                }

                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };
                ContestLeaderBoard.MakeLeaderBoard(10, contest.Teams, backColors, textColors, contest.FormatString);
                ContestLeaderBoard.Visible = true;
            }
        }

        protected void JoinContest(object sender, EventArgs e)
        {
            if(isValidId)
            {
                ContestManager.AddUser(contestId, user);
                LoadDateOnPage();
            }
        }
    }
}