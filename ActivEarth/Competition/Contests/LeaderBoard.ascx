<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeaderBoard.ascx.cs" Inherits="ActivEarth.Competition.Contests.LeaderBoard" %>

<asp:Table ID="LeaderBoardTable" runat="server" CssClass="contestTableText" BackColor="Black" CellSpacing="3" CellPadding="5">
    <asp:TableHeaderRow BackColor="White">
        <asp:TableHeaderCell Text="Bracket"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Team Name"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Score"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Reward"></asp:TableHeaderCell>
    </asp:TableHeaderRow>
</asp:Table>
