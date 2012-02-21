using System;
using System.Collections.Generic;
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
            var users = TestDAO.GetUserNames();
            lblMainUserName.Text = users.Rows[1]["user_name"].ToString();

        }
    }
}
