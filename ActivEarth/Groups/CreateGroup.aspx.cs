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
            
        }

        protected void Cancel(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups/Groups.aspx");
        }

        protected void AddHashTag(object sender, EventArgs e)
        {
            if (!lblAllHashTags.Text.ToLower().Contains(" " + txbHashTag.Text.ToLower() + ",") &&
                !lblAllHashTags.Text.ToLower().Contains(" " + txbHashTag.Text.ToLower() + ".")) {
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

        protected void CreateNewGroup(object sender, EventArgs e)
        {
            

            if (GroupDAO.GetGroupFromName(txbGroupName.Text) == null)
            {
                List<string> hashTags = lblAllHashTags.Text.Split(',').Select(sValue => sValue.Trim()).ToList();
                string last = hashTags.Last().TrimEnd('.');
                hashTags.RemoveAt(hashTags.Count - 1);
                hashTags.Add(last);



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