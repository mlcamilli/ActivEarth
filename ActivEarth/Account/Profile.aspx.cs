using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service;

namespace ActivEarth.Account
{
    /// <summary>
    /// This class represent the User Profile
    /// </summary>
    public partial class Profile : System.Web.UI.Page
    {
        /// <summary>
        /// Load the page
        /// </summary>
        /// <param name="sender">Object that requested the page load.</param>
        /// <param name="e">The event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userDetails"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                // get the user that is logged in.
                var userDetails = (User) Session["userDetails"];

                // update label on the profile tab
                lblUserName.Text = userDetails.UserName;
                lblFirstName.Text = userDetails.FirstName;
                lblLastName.Text = userDetails.LastName;
                lblEmail.Text = userDetails.Email;
                lblGender.Text = (userDetails.Gender == "M") ? "Male" : "Female";
                lblCityState.Text = userDetails.City + ", " + userDetails.State;
                lblAge.Text = userDetails.Age.ToString();
                lblHeight.Text = userDetails.Height.ToString();
                lblWeight.Text = userDetails.Weight.ToString();
            }

        }
    }
}