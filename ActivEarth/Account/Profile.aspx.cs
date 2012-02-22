using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                var userDetails = (DataTable) Session["userDetails"];
                lblUserName.Text = userDetails.Rows[0]["user_name"].ToString();
                lblFirstName.Text = userDetails.Rows[0]["first_name"].ToString();
                lblLastName.Text = userDetails.Rows[0]["last_name"].ToString();
                lblGender.Text = (userDetails.Rows[0]["gender"].ToString() == "M") ? "Male" : "Female";
                lblCityState.Text = userDetails.Rows[0]["city"].ToString() + ", " + userDetails.Rows[0]["state"].ToString();

            }

        }
    }
}