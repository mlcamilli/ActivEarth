using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;
using ActivEarth.Objects.Profile;

namespace ActivEarth.Account
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void CreateUser(object sender, EventArgs e)
        {
            if (UserDAO.GetUserFromUserName(txbUserName.Text) == null)
            {
                var user = new User { UserName = txbUserName.Text, FirstName = txbFirstName.Text, LastName = txbLastName.Text, Gender = Char.Parse(ddlGender.SelectedValue)};
                UserDAO.CreateNewUser(user, txbPassword.Text);
                Response.Redirect("RegisterConfirmation.aspx");
            }
            

            //UserDAO.CreateNewUser() 

        }

    }
}
