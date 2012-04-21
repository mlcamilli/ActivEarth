using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.DAO;

namespace ActivEarth.Competition.Contests
{
    public partial class DisplayContestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

                    if (contest.Mode == ContestEndMode.TimeBased)
                    {
                        TimeGraph.PopulateTimeGraph(contest.StartTime, contest.EndCondition.EndTime);
                        TimeGraph.Visible = true;
                    }
                    else
                    {
                        if (contest.Teams.Count >= 3)
                        {
                            GoalGraph.PopulateContestGraph(contest.Teams[0], contest.Teams[1], contest.Teams[2], contest.Teams[2], contest.EndCondition);
                        }
                        else
                        {
                            GoalGraph.PopulateContestGraph(contest.Teams[0], contest.Teams[1], contest.Teams[2], contest.EndCondition);
                        }
                        
                        GoalGraph.Visible = true;
                    }

                    Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                    Color[] textColors = { Color.White, Color.Black };
                    ContestLeaderBoard.MakeLeaderBoard(10, contest.Teams, backColors, textColors);
                    ContestLeaderBoard.Visible = true;
                }
            }  
        }
    }
}