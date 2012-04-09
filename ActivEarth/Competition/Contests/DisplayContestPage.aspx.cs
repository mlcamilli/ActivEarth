using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Competition.Contests
{
    public partial class DisplayContestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            _ContestName.Text = "Super Awesome Happy Funtime Contest";

                //Test Code
                List<Team> teams = new List<Team>();
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                teams.Add(new Team("C"));
                teams.Add(new Team("D"));
                teams.Add(new Team("E"));
                teams.Add(new Team("F"));
                teams.Add(new Team("G"));
                //End Test Code

            populateLeaderBoard(10, teams);
        }

        private void populateLeaderBoard(int slots, List<Team> teams)
        {
            Color[] backColors = {Color.FromArgb(34, 139, 34), Color.White};
            Color[] textColors = {Color.White, Color.Black};
            _leaderBoard.makeLeaderBoard(slots, teams, backColors, textColors);
        }
    }
}