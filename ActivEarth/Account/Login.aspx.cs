﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;

namespace ActivEarth.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
         
        }

        /// <summary>
        /// Login to the website
        /// </summary>
        /// <param name="sender">Object that requested the page load</param>
        /// <param name="e">The event arguments.</param>
        protected void LoginUser(object sender, EventArgs e)
        {
            var userDetails = UserDAO.GetUserFromUserNameAndPassword(tbUserName.Text, tbPassword.Text);
            if (userDetails == null)
            {
                Session["userDetails"] = null;
                lblError.Text = "Invalid Username / Password combination. Please try again.";
                tbPassword.Text = "";
            }else
            {
                Session["userDetails"] = userDetails;
                Response.Redirect("~/Account/Profile.aspx");
            }
        }
    }
}
