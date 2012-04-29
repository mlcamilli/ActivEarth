using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ActivEarth.Groups
{
    public partial class CreateGroup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void AddHashTag(object sender, EventArgs e)
        {
            if (!lblAllHashTags.Text.ToLower().Contains(" " + txbHashTag.Text.ToLower() + ",") &&
                !lblAllHashTags.Text.ToLower().Contains(" " + txbHashTag.Text.ToLower() + ".") {
                if (lblAllHashTags.Text == "")
                {
                    lblAllHashTags.Text = txbHashTag.Text + ".";
                }
                else
                {
                    lblAllHashTags.Text = lblAllHashTags.Text + ", " + txbHashTag.Text;
                }
            }

        }

        protected void CreateNewGroup(object sender, EventArgs e)
        {

        }

    }
}