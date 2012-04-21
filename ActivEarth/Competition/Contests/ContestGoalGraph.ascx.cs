using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Competition.Contests
{
    public partial class ContestGraph : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void PopulateContestGraph(Team firstPlace, Team secondPlace, Team currentUsersTeam, EndCondition goal)
        {
            _FirstPlaceTeamName.Text = firstPlace.Name;
            _SecondPlaceTeamName.Text = secondPlace.Name;
            _ThirdPlaceTeamName.Visible = false;
            _CurrentUserName.Text = currentUsersTeam.Name;

            _FirstPlaceProgress.Value = (int)((firstPlace.Score / goal.EndValue) * 100);
            _SecondPlaceProgress.Value = (int)((secondPlace.Score / goal.EndValue) * 100);
            _ThirdPlaceProgress.Visible = false;
            _CurrentUserProgress.Value = (int)((currentUsersTeam.Score / goal.EndValue) * 100);
        }

        public void PopulateContestGraph(Team firstPlace, Team secondPlace, Team thirdPlace, Team currentUsersTeam, EndCondition goal)
        {
            _FirstPlaceTeamName.Text = firstPlace.Name;
            _SecondPlaceTeamName.Text = secondPlace.Name;
            _ThirdPlaceTeamName.Text = thirdPlace.Name;
            _CurrentUserName.Text = currentUsersTeam.Name;

            _FirstPlaceProgress.Value = (int)((firstPlace.Score / goal.EndValue) * 100);
            _SecondPlaceProgress.Value = (int)((secondPlace.Score / goal.EndValue) * 100);
            _ThirdPlaceProgress.Value = (int)((thirdPlace.Score / goal.EndValue) * 100);
            _CurrentUserProgress.Value = (int)((currentUsersTeam.Score / goal.EndValue) * 100);
        }
    }
}