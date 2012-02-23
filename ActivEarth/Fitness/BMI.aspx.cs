using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActivEarth.Objects;

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

                var bmi = (double) ((user.Weight*703)/(user.Height*user.Height)) ;
                if (bmi < 18.5)
                {
                    lblBMIResult.Text = "underweight";
                }
                else if (bmi >= 18.5 && bmi < 25)
                {
                    lblBMIResult.Text = "normal";
                }
                else if (bmi >= 25 && bmi < 30)
                {
                    lblBMIResult.Text = "overweight";
                }
                else
                {
                    lblBMIResult.Text = "obese";
                }

                lblBMI.Text = bmi.ToString();

            }

        }
    }
}