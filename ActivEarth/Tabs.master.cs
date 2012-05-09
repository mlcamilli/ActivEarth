using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivEarth
{
    public partial class Tabs : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = Request.Url.ToString();

            if (url.Contains("Account"))
            {
                NavigationMenu.Items[0].Selected = true;
            }
            else if (url.Contains("Groups"))
            {
                NavigationMenu.Items[1].Selected = true;
            }
            else if (url.Contains("Badges"))
            {
                NavigationMenu.Items[2].Selected = true;
            }
            else if (url.Contains("Challenges"))
            {
                NavigationMenu.Items[3].Selected = true;
            }
            else if (url.Contains("Contests"))
            {
                NavigationMenu.Items[4].Selected = true;
            }
        }
    }
}