using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using ActivEarth.Objects.Competition.Contests;

namespace ActivEarth.Competition.Contests
{
    public partial class ContestList : System.Web.UI.UserControl
    {
        private ContestListRow[] ContestListRows;

        public ContestListRow[] Rows
        {
            get;
            set;
        }

        public void makeContestList()
        {
        }

        private void AddRowToContestList(string contestName, string score, Color backColor, Color textColor)
        {
            ContestListRow contestListRow = (ContestListRow)LoadControl("ContestListRow.ascx");
            contestListRow.CreateRow(contestName, backColor, textColor);

            _displayContestListRows.Controls.Add(contestListRow);
        }
    }
}