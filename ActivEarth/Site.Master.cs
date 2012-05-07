using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects;
using ActivEarth.Objects.Profile;
using ActivEarth.DAO;
using System.Drawing;
using ActivEarth.Objects.Groups;
using ActivEarth.Server.Service.Statistics;

namespace ActivEarth
{
    /// <summary>
    /// This class represent the Site Master page, used after user logged in.
    /// </summary>
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        /// <summary>
<<<<<<< HEAD
        /// Load the page
        /// </summary>
        /// <param name="sender">Object that requested the page load.</param>
        /// <param name="e">The event arguments.</param>
=======
        /// Prepares the display of the User's Image, Name, Recent Activity, Weather information, and Statistics
        /// when the page loads.
        /// </summary>
>>>>>>> 5e28b2b29a16993fcb3516eef7f898502c498b1d
        protected void Page_Load(object sender, EventArgs e)
        {
            // if user is not logged in 
            if (Session["userDetails"] == null)
            {
                lbLogOut.Visible = false;
                lblUserLoggedIn.Visible = false;
            }
            else
            {
                var userDetails = (User) Session["userDetails"];
                hlRegister.Visible = false;
                hlLogin.Visible = false;
                lbLogOut.Visible = true;
                lblUserLoggedIn.Visible = true;
                lblUserLoggedIn.Text = "Logged in as " + userDetails.UserName;

                lblUserName.Text = userDetails.FirstName + " " + userDetails.LastName;
                userIconImage.ImageUrl = getUserImageUrl(userDetails, "icon");

                List<Message> messages = userDetails.Wall.Messages;
                Color[] backColors = {Color.White, Color.FromArgb(34, 139, 34)};
                Color[] textColors = {Color.Black, Color.White };
                RecentActivityTable.PopulateMessageTable(messages, backColors, textColors);

                lblActivityScore.Text = userDetails.ActivityScore.TotalScore.ToString();
                lblGreenScore.Text = userDetails.GreenScore.ToString();
                StatisticManager userStat = new StatisticManager(userDetails);
                lblStatGasSavings.Text = "$ " + userStat.GetUserStatistic(Statistic.GasSavings).Value;

                
                if (userDetails.City != null)
                {
                    if (DisplayWeatherControl1.GetCurrentConditions(userDetails.City.Replace(' ', '+')))
                    {
                        CityNotFound.Text = "";
                    }
                    else
                    {
                        DisplayWeatherControl1.Visible = false;
                        CityNotFound.Text = "City was not found.  Please edit the city in your profile information to view Weather updates.";
                    }
                }
                else
                {
                    DisplayWeatherControl1.Visible = false ;
                    CityNotFound.Text = "Please enter a city into your profile information to view Weather updates.";
                }
            }

        }
        
        /// <summary>
<<<<<<< HEAD
        /// Log out the current user
        /// </summary>
        /// <param name="sender">Object that requested the page load</param>
        /// <param name="e">The event arguments.</param>
=======
        /// Method called when the User clicks the Log Out link.  Removes the Users session information 
        /// and redirects to the Home Page.
        /// </summary>
>>>>>>> 5e28b2b29a16993fcb3516eef7f898502c498b1d
        protected void UserLogOut(object sender, EventArgs e)
        {
            Session["userDetails"] = null;
            Response.Redirect("~/Default.aspx");
        }

        /// <summary>
        /// Returns the relative url for an image.
        /// 
        /// Current image sizes are:
        ///     - icon: a 150x150 image for the user's profile
        /// </summary>
        /// <param name="user">The user to retrieve the image for.</param>
        /// <param name="imageSizeName">The name of the image size to retrieve.</param>
        /// <returns></returns>
        public string getUserImageUrl(User user, string imageSizeName)
        {
            string path = Server.MapPath("~") + "\\Images\\Account\\UserProfile\\" + imageSizeName + "\\";
            int userImageDir = (user.UserID / 1000);
            string uploadPath = String.Format("{0}\\{1}\\{2}.png", path, userImageDir, user.UserID);

            if (System.IO.File.Exists(uploadPath))
            {
                return String.Format("/Images/Account/UserProfile/{0}/{1}/{2}.png", imageSizeName, userImageDir, user.UserID);
            }
            else
            {
                return String.Format("/Images/Account/UserProfileDefaults/default_{0}.png", imageSizeName);
            }
        }

        /// <summary>
        /// Method called when the Post Button is clicked.  Adds a Message to the User's Recent Activity using the text in txbTitle
        /// and txbMessage and including a time stamp.
        /// </summary>
        protected void PostMessage(object sender, EventArgs e)
        {
            if (txbTitle.Text != "" && txbMessage.Text != "")
            {
                User user = (User)Session["userDetails"];

                string[] dateTime = DateTime.Now.ToString("MM/dd/yyyy h:mmtt").Split(' ');
                user.Post(new Message(txbTitle.Text, txbMessage.Text, user, dateTime[0], dateTime[1]));

                if (UserDAO.UpdateUserProfile(user))
                {
                    Session["userDetails"] = user;
                }

                Response.Redirect(Request.RawUrl);
            }
        }
    }
}
