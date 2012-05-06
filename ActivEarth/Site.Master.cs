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
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
