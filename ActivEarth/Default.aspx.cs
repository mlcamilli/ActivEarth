using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;

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
                var userDetails = (DataTable)Session["userdetails"];
                lblMainUserName.Text = userDetails.Rows[0]["first_name"].ToString() + " " +
                                       userDetails.Rows[0]["last_name"];
            }
            

        }
    }
}
