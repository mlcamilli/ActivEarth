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
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userDetails"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void BtnCancelClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Account/Profile.aspx");
        }

        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            if (NewPassword.Text.Length < 6 || NewPassword.Text.Length > 20)
            {
                FailureText.Text = "Password must be between 6 and 20 characters long";
            }
            else
            {
                var user = (Session["userDetails"] as User);
                if (user != null && TestDAO.ConfirmPassword(CurrentPassword.Text, user.UserID))
                {
                    TestDAO.UpdatePassword(NewPassword.Text, user.UserID);
                    Response.Redirect("~/Account/ChangePasswordSuccess.aspx");
                }
                else
                {
                    FailureText.Text = "An error occurred attempting to change your password.";
                }
            }
        }
    }
}
