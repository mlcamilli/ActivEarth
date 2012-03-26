using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ActivEarth.Objects;
using ActivEarth.Objects.Profile;

namespace ActivEarth.Fitness
{
    public partial class BMI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = (User)Session["userDetails"];
            if (user == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else if (user.Height == null || user.Weight == null)
            {
                pnlBMI.Visible = false;
                pnlNotEnoughData.Visible = true;
            }
            else
            {
                pnlBMI.Visible = true;
                pnlNotEnoughData.Visible = false;

                var bmi = BMICalculator.CalculateBMI((int)user.Height, (int)user.Weight) ;
                lblBMI.Text = bmi.ToString();
                lblBMIResult.Text = BMICalculator.GetBMIResult(bmi);
            }

        }
    }
}