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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (lblAllHashTags.Text == ""){
                lblAllHashTags.Text = "[]";
            }
        }

        protected void Cancel(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups/Groups.aspx");
        }

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

        protected void RemoveHashTags(object sender, EventArgs e)
        {
            lblAllHashTags.Text = "[]";
        }

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