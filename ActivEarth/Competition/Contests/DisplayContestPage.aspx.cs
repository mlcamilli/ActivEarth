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
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                string contestIdString = Request.QueryString["id"];
                int id;
                if (contestIdString != null && int.TryParse(contestIdString, out id))
                {
                    Contest contest = ContestDAO.GetContestFromContestId(id);
                    if (contest != null)
                    {
                        ContestName.Text = contest.Name;
                        ContestDescription.Text = contest.Description;
                        ContestActivityScore.Text = contest.Reward.ToString();

                        if (contest.Mode == ContestEndMode.TimeBased)
                        {
                            TimeGraph.PopulateTimeGraph(contest.StartTime, contest.EndCondition.EndTime);
                            TimeGraph.Visible = true;
                        }
                        else
                        {
                            if (contest.Teams.Count >= 3)
                            {
                                GoalGraph.PopulateContestGraph(contest.Teams[0], contest.Teams[1], contest.Teams[2], contest.Teams[2], contest.EndCondition.EndValue);
                            }
                            else
                            {
                                GoalGraph.PopulateContestGraph(contest.Teams[0], contest.Teams[1], contest.Teams[2], contest.EndCondition);
                            }

                            GoalGraph.SetGraphLabels(contest.EndCondition.EndValue, contest.FormatString);
                            GoalGraph.Visible = true;
                        }

                        Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                        Color[] textColors = { Color.White, Color.Black };
                        ContestLeaderBoard.MakeLeaderBoard(10, contest.Teams, backColors, textColors, contest.FormatString);
                        ContestLeaderBoard.Visible = true;
                    }
                }
            }
        }
    }
}