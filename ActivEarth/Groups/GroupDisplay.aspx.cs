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
        /// <summary>
        /// Prepares the Group name, Group description, list of Hashtags, Green score and Activity Score, 
        /// preview User table, Contests table, and Group Wall table to display when the page loads.  Redirects 
        /// the user if they have not signed in or if a Group ID has not been provided.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["userDetails"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");

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
                lblGreenScore.Text = currentGroup.GreenScore.ToString();
                lblActivityScore.Text = currentGroup.ActivityScore.TotalScore.ToString();
                lblHashTags.Text = "[";
                int i = 0;
                foreach (string tag in currentGroup.HashTags)
                {
                    if (i == 0)
                    {
                        lblHashTags.Text = lblHashTags.Text + tag;
                    }
                    else
                    {
                        lblHashTags.Text = lblHashTags.Text + ", " + tag;
                    }
                    i++;
                }
                lblHashTags.Text = lblHashTags.Text + "]";

                hypSeeMore.NavigateUrl = "~/Groups/MembersPage.aspx?ID=" + groupID;


                List<ActivEarth.Objects.Profile.User> membersList = currentGroup.Members;
                Color[] backColors = { Color.FromArgb(34, 139, 34), Color.White };
                Color[] textColors = { Color.White, Color.Black };
                MembersDisplayTable1.PopulateMembersTable_Display(membersList, backColors, textColors);


                List<ActivEarth.Objects.Groups.Message> messageList = currentGroup.Wall.Messages;
                WallDisplay1.PopulateMessageTable(messageList, backColors, textColors);


                List<int> contestIdList = ContestDAO.GetContestIdsFromGroupId(groupID);
                List<string> contestNameList = new List<string>();

                foreach (int id in contestIdList)
                {
                    contestNameList.Add(ContestDAO.GetContestNameFromContestId(id));
                }

                if (contestNameList.Count > 0)
                {
                    ContestDisplayTable1.PopulateContestTable(contestNameList, contestIdList, backColors, textColors);
                    ContestDisplayTable1.Visible = true;
                    EmptyContest.Visible = false;
                }
                else
                {
                    ContestDisplayTable1.Visible = false;
                    EmptyContest.Visible = true;
                }
            }
        }

        /// <summary>
        /// Method called when the Post Button is clicked.  Adds a Message to the Group Wall using the text in txbTitle
        /// and txbMessage and including a time stamp.
        /// </summary>
        protected void PostMessage(object sender, EventArgs e){

            int groupID = Convert.ToInt32(Request.QueryString["ID"]);
            Group group = GroupDAO.GetGroupFromGroupId(groupID);

            if (txbTitle.Text != "" && txbMessage.Text != "")
            {
                User user = (User)Session["userDetails"];

                string[] dateTime = DateTime.Now.ToString("MM/dd/yyyy h:mmtt").Split(' ');
                user.Post(new Message(txbTitle.Text, txbMessage.Text, user, dateTime[0], dateTime[1]));

                group.Post(new Message(txbTitle.Text, txbMessage.Text, user, dateTime[0], dateTime[1]));
                GroupDAO.UpdateGroup(group);
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}