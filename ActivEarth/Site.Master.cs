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
            }else
            {
                var userDetails = (User) Session["userDetails"];
                hlRegister.Visible = false;
                hlLogin.Visible = false;
                lbLogOut.Visible = true;
                lblUserLoggedIn.Visible = true;
                lblUserLoggedIn.Text = "Logged in as " + userDetails.UserName;

                lblUserName.Text = userDetails.FirstName + " " + userDetails.LastName;
                lblCityState.Text = userDetails.City + ", " + userDetails.State;

            }

        }
        
        protected void UserLogOut(object sender, EventArgs e)
        {
            Session["userDetails"] = null;
            Response.Redirect("~/Default.aspx");
        }
    }
}
