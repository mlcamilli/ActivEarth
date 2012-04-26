using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects;
using ActivEarth.Objects.Profile;

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
                lblCityState.Text = userDetails.City + ", " + userDetails.State;
                userIconImage.ImageUrl = getUserImageUrl(userDetails, "icon");
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
        private string getUserImageUrl(User user, string imageSizeName)
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
    }
}
