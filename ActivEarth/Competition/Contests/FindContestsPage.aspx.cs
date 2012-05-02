using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.DAO;
using ActivEarth.Server.Service.Competition;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Competition.Contests
{
    /// <summary>
    /// The class represents the find contests page on the 
    /// ActivEarth website.
    /// </summary>
    public partial class FindContestsPage : System.Web.UI.Page
    {
        User user;

        /// <summary>
        /// Loas the page.
        /// </summary>
        /// <param name="sender">Object that sent the page request.</param>
        /// <param name="e">Event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        /// <summary>
        /// Queries the database for contest based on the 
        /// search text.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected void ExecuteSearch(object sender, EventArgs e)
        {
            List<int> contestIds = ContestDAO.FindContests(txtSearchText.Text, chkExactMatch.Checked);
            List<string> contestNames = new List<string>();

            if (contestIds.Count > 0)
            {
                foreach (int id in contestIds)
                {
                    contestNames.Add(ContestDAO.GetContestNameFromContestId(id));
                }


                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };

                SearchResults.Visible = true;
                lblNoResultsFound.Visible = false;
                SearchResults.PopulateContestTable(contestNames, contestIds, backColors, textColors);
            }
            else
            {
                lblNoResultsFound.Visible = true;
                SearchResults.Visible = false;
                lblNoResultsFound.Text = String.Format("Sorry, no contests found with \"{0}\".", txtSearchText.Text);
            }
        }
    }
}