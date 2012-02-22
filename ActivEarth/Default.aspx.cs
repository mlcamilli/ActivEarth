using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;
using ActivEarth.Objects;
using ActivEarth.Server.Service;

namespace ActivEarth
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {   
            if(Session["userdetails"] == null)
            {
                lblMainUserName.Text = "Please Login";
            }else
            {
                var userDetails = (User)Session["userdetails"];
                lblMainUserName.Text = userDetails.FirstName + " " + userDetails.LastName;
            }
        }
    }
}
