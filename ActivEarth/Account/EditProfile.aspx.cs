using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;
using ActivEarth.Objects;

namespace ActivEarth.Account
{
    public partial class EditProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userDetails"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                var userDetails = (User)Session["userDetails"];
                lblUserName.Text = userDetails.UserName;
                tbFirstName.Text = userDetails.FirstName;
                tbLastName.Text = userDetails.LastName;
                tbEmail.Text = userDetails.Email;
                ddlGender.SelectedValue = userDetails.Gender + "";
                tbCity.Text = userDetails.City;
                tbState.Text = userDetails.State;
            }

        }
        protected void SaveUserProfile(object sender, EventArgs e)
        {
            var user = new User
                           {
                               UserID = ((User) Session["userDetails"]).UserID,
                               FirstName = tbFirstName.Text,
                               LastName = tbLastName.Text,
                               Gender = Char.Parse(ddlGender.SelectedValue),
                               Email =  tbEmail.Text,
                               City =  tbCity.Text,
                               State = tbState.Text
                           };
            if (TestDAO.UpdateUserProfile(user))
            {
                Session["userDetails"] = TestDAO.GetUserFromUserId(user.UserID);  
                Response.Redirect("~/Account/Profile.aspx");
            }else
            {
                Response.Redirect("~/Account/Profile.aspx");    
            }
            
            



        }
    }
}