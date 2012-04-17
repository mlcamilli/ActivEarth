using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ActivEarth.Competition.Contests
{
    public partial class ContestListRow : System.Web.UI.UserControl
    {
        public Color BackColor
        {
            get
            {
                return _row.BackColor;
            }

            set
            {
                _contestName.ForeColor = value;
                _row.BackColor = value;
            }
        }

        public void CreateRow(string contestName, Color backColor, Color textColor)
        {
            _contestName.Text = contestName;
            _contestName.ForeColor = textColor;
            _row.BackColor = backColor;
        }
    }
}