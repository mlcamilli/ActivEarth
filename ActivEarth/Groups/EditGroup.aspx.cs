using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;

namespace ActivEarth.Groups
{
    public partial class EditGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["userDetails"] == null)
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

                

                if (currentGroup == null || currentGroup.Owner.UserID != ((User)Session["userDetails"]).UserID)
                {
                    Response.Redirect("~/Groups/Groups.aspx");
                }

                if (txbGroupName.Text == "")
                {
                    txbGroupName.Text = currentGroup.Name;
                }
                if (txbDescription.Text == "")
                {
                    txbDescription.Text = currentGroup.Description;
                }
                if (lblAllHashTags.Text == "" && txbHashTag.Text == "")
                {
                    foreach (string tag in currentGroup.HashTags)
                    {
                        txbHashTag.Text = tag;
                        AddHashTag(null, null);
                    }

                    txbHashTag.Text = "";
                }

                
            }
        }

        protected void Cancel(object sender, EventArgs e)
        {

            Response.Redirect("~/Groups/Groups.aspx");
        }

        protected void DeleteGroup(object sender, EventArgs e)
        {
            int groupID = Convert.ToInt32(Request.QueryString["ID"]);
            GroupDAO.DeleteGroup(groupID);
            Response.Redirect("~/Groups/Groups.aspx");
        }

        protected void AddHashTag(object sender, EventArgs e)
        {
            if (!lblAllHashTags.Text.ToLower().Contains(" " + txbHashTag.Text.ToLower() + ",") &&
                !lblAllHashTags.Text.ToLower().Contains(" " + txbHashTag.Text.ToLower() + "."))
            {
                if (lblAllHashTags.Text == "")
                {
                    lblAllHashTags.Text = txbHashTag.Text + ".";
                }
                else
                {
                    lblAllHashTags.Text = lblAllHashTags.Text.Substring(0, lblAllHashTags.Text.Length - 1) + ", " + txbHashTag.Text + ".";
                }
            }
        }

        protected void RemoveHashTags(object sender, EventArgs e)
        {
            lblAllHashTags.Text = "";
        }

        protected void EditGroupInformation(object sender, EventArgs e)
        {

            if (GroupDAO.GetGroupFromName(txbGroupName.Text) == null)
            {
                List<string> hashTags = lblAllHashTags.Text.Split(',').Select(sValue => sValue.Trim()).ToList();
                string last = hashTags.Last().TrimEnd('.');
                hashTags.RemoveAt(hashTags.Count - 1);
                hashTags.Add(last);

                int groupID = Convert.ToInt32(Request.QueryString["ID"]);
                Group group = GroupDAO.GetGroupFromGroupId(groupID);

                group.Name = txbGroupName.Text;
                group.Description = txbDescription.Text;
                group.HashTags = hashTags;

                GroupDAO.UpdateGroup(group);

                    
                Response.Redirect("~/Groups/Groups.aspx");
                
            }
            else
            {
                ErrorMessage.Text = "A Group with the given Group Name already exists.  Please choose a different Group Name.";
            }
        }
    }
}