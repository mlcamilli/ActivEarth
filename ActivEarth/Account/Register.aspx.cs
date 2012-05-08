using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;
using ActivEarth.Objects.Profile;
using ActivEarth.Objects.Groups;

namespace ActivEarth.Account
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// Create a new weather.
        /// </summary>
        /// <param name="sender">Object that requested the page load</param>
        /// <param name="e">The event arguments.</param>
        protected void CreateUser(object sender, EventArgs e)
        {
            if (UserDAO.GetUserFromUserName(txbUserName.Text) == null)
            {
                var user = new User { UserName = txbUserName.Text, FirstName = txbFirstName.Text, LastName = txbLastName.Text, Email = txbEmail.Text, Gender = (ddlGender.SelectedValue) };
                user.UserID = UserDAO.CreateNewUser(user, txbPassword.Text);

                string[] dateTime = DateTime.Now.ToString("MM/dd/yyyy h:mmtt").Split(' ');
                user.Post(new Message("Welcome to ActivEarth!", "See the tabs to the right to start you on your way to a more active lifestyle!", 
                    user, dateTime[0], dateTime[1]));
                UserDAO.UpdateUserProfile(user);

                if (user.UserID == 0) {
                    ErrorMessage.Text = "A Database error occurred creating the User.";
                }
                else{
                    Session["userDetails"] = user;
                    Response.Redirect("~/Groups/Groups.aspx");
                }
            }
            else
            {
                ErrorMessage.Text = "A User with the given User Name already exists.  Please choose a different User Name.";
            }
            

            //UserDAO.CreateNewUser() 

        }

    }
}
