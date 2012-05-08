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
        /// <summary>
        /// Populates the text fields on the page with the Group's current information when the page loads.  
        /// Redirects if the User is not logged in, the ID of the Group was not provided, or if the User is
        /// not the Owner of the Group.
        /// </summary>
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
                if (lblAllHashTags.Text == "")
                {
                    lblAllHashTags.Text = "[]";
                    foreach (string tag in currentGroup.HashTags)
                    {
                        txbHashTag.Text = tag;
                        AddHashTag(null, null);
                    }                  
                }

            }
        }

        /// <summary>
        /// Method called when the Cancel Button is clicked.  Redirects the user
        /// back to the Groups page.
        /// </summary>
        protected void Cancel(object sender, EventArgs e)
        {

            Response.Redirect("~/Groups/Groups.aspx");
        }

        /// <summary>
        /// Method called when the Delete Group Button is clicked.  Removes the Group from
        /// the Database and Redirects the user back to the Groups page.
        /// </summary>
        protected void DeleteGroup(object sender, EventArgs e)
        {
            int groupID = Convert.ToInt32(Request.QueryString["ID"]);
            GroupDAO.DeleteGroup(groupID);
            Response.Redirect("~/Groups/Groups.aspx");
        }

        /// <summary>
        /// Method called when the Add Hashtag Button is clicked.  Adds the text in txbHashTag
        /// to the list of Hashtags and clears the text in txbHashtag.
        /// </summary>
        protected void AddHashTag(object sender, EventArgs e)
        {
            lblAllHashTags.Text = lblAllHashTags.Text.Substring(1, lblAllHashTags.Text.Length - 2);
            if (lblAllHashTags.Text == "")
            {
                lblAllHashTags.Text = "[" + txbHashTag.Text + "]";
            }
            else
            {
                lblAllHashTags.Text = "[" + lblAllHashTags.Text + ", " + txbHashTag.Text + "]";
            }
            
            txbHashTag.Text = "";
        }

        /// <summary>
        /// Method called when the Remove Hashtags Button is clicked.  Clears the list of Hashtags.
        /// </summary>
        protected void RemoveHashTags(object sender, EventArgs e)
        {
            lblAllHashTags.Text = "[]";
        }

        /// <summary>
        /// Method called when the Manage Group Members Button is clicked.  Redirects the Owner to the 
        /// Manage Group Members Page.
        /// </summary>
        protected void BootMembers(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups/OwnerMembersPage.aspx?ID=" + Request.QueryString["ID"]);
        }

        /// <summary>
        /// Method called when the Save Group Information Button is clicked.  Pulls information from the text boxes on the page
        /// and updates the group information in the Database.
        /// </summary>
        protected void EditGroupInformation(object sender, EventArgs e)
        {
            int groupID = Convert.ToInt32(Request.QueryString["ID"]);
            Group group = GroupDAO.GetGroupFromGroupId(groupID);

            Group test = GroupDAO.GetGroupFromName(txbGroupName.Text);
            if (test == null || test.ID == groupID)
            {
                List<string> hashTags = lblAllHashTags.Text.Substring(1, lblAllHashTags.Text.Length - 2).Split(',').Select(sValue => sValue.Trim()).ToList();
                    
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