<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeaderBoard.ascx.cs" Inherits="ActivEarth.Competition.Contests.LeaderBoard" %>
<%@ Reference Control="LeaderBoardRow.ascx" %>

<asp:Panel ID="_displayLeaderBoardRows" runat="server" Width="300px" 
    style="position: relative" BorderColor="Black" BorderStyle="Solid" BorderWidth="3px">

    <asp:Panel ID="_header" runat="server" Height="25px" 
    Width="300px" style="position: relative">

        <asp:Label ID="_leaderBoardTitle" runat="server" Text="Contest Rankings"       
            style="z-index: 1; left: 6px; top: 3px; position: absolute; width: 227px;" 
            Font-Names="Eras Bold ITC" Font-Size="Medium" ForeColor="Black"></asp:Label>
        <asp:ImageButton ID="nextRankings" runat="server" 
            onclick="nextRankings_Click" 
            ImageUrl="~/Images/Competition/Misc/RightArrow.png" 
            style="z-index: 1; left: 278px; top: 3px; position: absolute" />
        <asp:ImageButton ID="previousRankings" runat="server" 
            onclick="previousRankings_Click" 
            ImageUrl="~/Images/Competition/Misc/LeftArrow.png" 
            style="z-index: 1; left: 252px; top: 3px; position: absolute" />
    </asp:Panel>

</asp:Panel>
