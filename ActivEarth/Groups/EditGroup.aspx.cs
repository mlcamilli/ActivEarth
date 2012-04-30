using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.DAO;
using ActivEarth.Objects.Groups;

namespace ActivEarth.Groups
{
    public partial class EditGroup : System.Web.UI.Page
    {
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

        protected void EditGroupValues(object sender, EventArgs e)
        {

            if (GroupDAO.GetGroupFromName(txbGroupName.Text) == null)
            {
                List<string> hashTags = lblAllHashTags.Text.Split(',').Select(sValue => sValue.Trim()).ToList();
                string last = hashTags.Last().TrimEnd('.');
                hashTags.RemoveAt(hashTags.Count - 1);
                hashTags.Add(last);

                //Group group = GroupDAO.GetGroupFromGroupId(groupId);
                //GroupDAO.UpdateGroup(group);

                    
                Response.Redirect("~/Groups/Groups.aspx");
                
            }
            else
            {
                ErrorMessage.Text = "A Group with the given Group Name already exists.  Please choose a different Group Name.";
            }
        }
    }
}