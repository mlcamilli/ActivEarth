using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivEarth
{
    public partial class GroupTabs : System.Web.UI.MasterPage
    {

        int groupID;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] == null)
            {
                Response.Redirect("~/Groups/Groups.aspx");
            }
            else
            {
                groupID = Convert.ToInt32(Request.QueryString["ID"]); 
            }
        }

        protected void GroupNavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "Wall":
                    Response.Redirect("~/Groups/WallPage.aspx?ID=" + groupID);
                    break;
                case "Members":
                    Response.Redirect("~/Groups/MembersPage.aspx?ID=" + groupID);
                    break;
                case "Contests":
                    Response.Redirect("~/Groups/ContestPage.aspx?ID=" + groupID);
                    break;
            }

        }
    }
}