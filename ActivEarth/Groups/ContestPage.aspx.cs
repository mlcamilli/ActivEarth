using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ActivEarth.Account;
using ActivEarth.Groups;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects;
using ActivEarth.Objects.Competition.Contests;
using ActivEarth.Server.Service;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service.Competition;
using ActivEarth.Competition.Contests;
using ActivEarth.DAO;

namespace ActivEarth.Groups
{
    public partial class ContestPage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["userDetails"] == null)
            {
                Response.Redirect("Login.aspx");

            }
            else if (Request.QueryString["ID"] == null)
            {
                Response.Redirect("~/Groups/Groups.aspx");
            }
            else
            {
                int groupID = Convert.ToInt32(Request.QueryString["ID"]);
                Group currentGroup = GroupDAO.GetGroupFromGroupId(groupID);

                lblGroupName.Text = currentGroup.Name;
                lblDescription.Text = currentGroup.Description;
                
                Color[] backColors = { Color.FromArgb(75, 108, 158), Color.White };
                Color[] textColors = { Color.White, Color.Black };

                List<Contest> contestList = currentGroup.Contests;
                List<int>    contestIdList = new List<int>();
                List<string> contestNameList = new List<string>();

                foreach (Contest contest in contestList)
                {
                    contestIdList.Add(contest.ID);
                    contestNameList.Add(contest.Name);
                }
                
                ContestDisplayTable1.PopulateContestTable(contestNameList, contestIdList, backColors, textColors); 


            }

        }
    }
}