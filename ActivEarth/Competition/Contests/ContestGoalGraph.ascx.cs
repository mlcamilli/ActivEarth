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

        public void PopulateContestGraph(Team firstPlace, Team secondPlace, Team thirdPlace, Team currentUsersTeam, float goal)
        {
            _FirstPlaceTeamName.Visible = (firstPlace != null);
            _FirstPlaceProgress.Visible = (firstPlace != null);
            _SecondPlaceTeamName.Visible = (secondPlace != null);
            _SecondPlaceProgress.Visible = (secondPlace != null);
            _ThirdPlaceTeamName.Visible = (thirdPlace != null);
            _ThirdPlaceProgress.Visible = (thirdPlace != null);
            _CurrentUserName.Visible = (currentUsersTeam != null);
            _CurrentUserProgress.Visible = (currentUsersTeam != null);

            _FirstPlaceTeamName.Text = (firstPlace != null ? firstPlace.Name : String.Empty);
            _SecondPlaceTeamName.Text = (secondPlace != null ? secondPlace.Name : String.Empty);
            _ThirdPlaceTeamName.Text = (thirdPlace != null ? thirdPlace.Name : String.Empty);
            _CurrentUserName.Text = (currentUsersTeam != null ? currentUsersTeam.Name : String.Empty);

            _FirstPlaceProgress.Value = (firstPlace != null ? (int)((firstPlace.Score / goal) * 100) : 0);
            _SecondPlaceProgress.Value = (secondPlace != null ? (int)((secondPlace.Score / goal) * 100) : 0);
            _ThirdPlaceProgress.Value = (thirdPlace != null ? (int)((thirdPlace.Score / goal) * 100) : 0);
            _CurrentUserProgress.Value = (currentUsersTeam != null ? (int)((currentUsersTeam.Score / goal) * 100) : 0);
        }

        public void SetGraphLabels(float goal, string format)
        {
            _StartLabel.Text = 0.ToString(format);
            _GoalLabel.Text = goal.ToString(format); 
        }
    }
}