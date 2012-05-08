using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects.Groups;
using ActivEarth.Objects.Profile;
using ActivEarth.DAO;

namespace ActivEarth.Groups
{
    public partial class CreateGroup : System.Web.UI.Page
    {
        /// <summary>
        /// Prepares the list of hashtags when the page loads.  Other fields are empty for the new Group.
        /// Redirects if the User is not logged in.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userDetails"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }

            if (lblAllHashTags.Text == ""){
                lblAllHashTags.Text = "[]";
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
        /// Method called when the Create Group Button is clicked.  Pulls information from the text boxes on the page, adds a welcome
        /// message to the Group wall, and inserts the new group into the Database.
        /// </summary>
        protected void CreateNewGroup(object sender, EventArgs e)
        {
            

            if (GroupDAO.GetGroupFromName(txbGroupName.Text) == null)
            {
                List<string> hashTags = lblAllHashTags.Text.Substring(1, lblAllHashTags.Text.Length - 2).Split(',').Select(sValue => sValue.Trim()).ToList();
    
                User owner = (User) Session["userDetails"];

                Group group = new Group(txbGroupName.Text, owner, txbDescription.Text, hashTags);
                group.ID = GroupDAO.CreateNewGroup(group);
                string[] dateTime = DateTime.Now.ToString("MM/dd/yyyy h:mmtt").Split(' ');
                group.Post(new Message("Created Group", owner.FirstName + " " + owner.LastName + " created the group " + group.Name + "!",
                    owner, dateTime[0], dateTime[1]));
                GroupDAO.UpdateGroup(group);
                        
                if (group.ID == 0)
                {
                    ErrorMessage.Text = "A Database error occurred creating the Group.";
                }
                else
                {
                    Response.Redirect("~/Groups/Groups.aspx");
                }
            }
            else
            {
                ErrorMessage.Text = "A Group with the given Group Name already exists.  Please choose a different Group Name.";
            }
        }

    }
}