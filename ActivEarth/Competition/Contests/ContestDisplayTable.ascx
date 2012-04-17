<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContestDisplayTable.ascx.cs" Inherits="ActivEarth.Competition.Contests.ContestDisplayTable" %>

<asp:Table ID="_contestTable" runat="server" BorderStyle="Solid" 
    BorderColor="Black" ForeColor="Black" Font-Names="Eras Bold ITC" 
    CellPadding="5" CellSpacing="5" GridLines="Vertical" BorderWidth="3px" 
    HorizontalAlign="Left">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell Text="Contest Name"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Type"></asp:TableHeaderCell>
    </asp:TableHeaderRow>
</asp:Table>