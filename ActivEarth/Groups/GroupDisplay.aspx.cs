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
using ActivEarth.Server.Service;
using ActivEarth.Objects.Profile;
using ActivEarth.Server.Service.Competition;
using ActivEarth.Competition.Contests;
using ActivEarth.DAO;

namespace ActivEarth.Groups
{
    public partial class GroupDisplay : System.Web.UI.Page
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

                List<ActivEarth.Objects.Profile.User> membersList = currentGroup.Members;
                Color[] backColors = { Color.FromArgb(75, 108, 158), Color.White };
                Color[] textColors = { Color.White, Color.Black };
                MembersDisplayTable1.PopulateMembersTable(membersList, backColors, textColors);


                List<ActivEarth.Objects.Groups.Message> messageList = currentGroup.Wall.Messages;
                WallDisplay1.PopulateMessageTable(messageList, backColors, textColors);


                List<int> contestIdList = ContestDAO.GetContestIdsFromGroupId(groupID);
                List<string> contestNameList = new List<string>();

                foreach (int id in contestIdList)
                {
                    contestNameList.Add(ContestDAO.GetContestNameFromContestId(id));
                }
                
                ContestDisplayTable1.PopulateContestTable(contestNameList, contestIdList, backColors, textColors);



            }

        }
    }
}