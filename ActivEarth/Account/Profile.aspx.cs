using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects;
using ActivEarth.Server.Service;

namespace ActivEarth.Account
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userDetails"] == null)
            {
                Response.Redirect("Login.aspx");
            }else
            {
                var userDetails = (User) Session["userDetails"];
                lblUserName.Text = userDetails.UserName;
                lblFirstName.Text = userDetails.FirstName;
                lblLastName.Text = userDetails.LastName;
                lblEmail.Text = userDetails.Email;
                lblGender.Text = (userDetails.Gender == 'M') ? "Male" : "Female";
                lblCityState.Text = userDetails.City + ", " + userDetails.State;
            }

        }
    }
}