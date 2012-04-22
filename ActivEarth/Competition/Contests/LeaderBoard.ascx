<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeaderBoard.ascx.cs" Inherits="ActivEarth.Competition.Contests.LeaderBoard" %>
<%@ Reference Control="LeaderBoardRow.ascx" %>

<asp:Panel ID="_displayLeaderBoardRows" runat="server" Width="300px" 
    style="position: relative" BorderColor="Black" BorderStyle="Solid" BorderWidth="3px">

    <asp:Panel ID="_header" runat="server" Height="25px" 
    Width="300px" style="position: relative">

        <asp:Label ID="_leaderBoardTitle" runat="server" Text="Contest Rankings"       
            style="z-index: 1; left: 6px; top: 3px; position: absolute; width: 227px;" ForeColor="Black"></asp:Label>
    </asp:Panel>

</asp:Panel>
