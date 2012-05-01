<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="GroupsDisplayTable.ascx.cs" Inherits="ActivEarth.Groups.GroupsDisplayTable" %>

<asp:Table ID="_groupsTable" runat="server" BorderStyle="Solid" 
    BorderColor="Black" ForeColor="Black" Font-Names="Georgia" 
    CellPadding="3" CellSpacing="3" GridLines="Both" BorderWidth="2px" 
    HorizontalAlign="Left">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell Text="Group Name"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Description"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Owner"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Activity Score"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Green Score"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Leave or Join?"></asp:TableHeaderCell>
    </asp:TableHeaderRow>
</asp:Table>