<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="MembersDisplayTable.ascx.cs" Inherits="ActivEarth.Groups.MembersDisplayTable" %>

<asp:Table ID="_usersTable" runat="server" BorderStyle="Solid" 
    BorderColor="Black" ForeColor="Black" Font-Names="Georgia" 
    CellPadding="3" CellSpacing="3" GridLines="Both" BorderWidth="2px" 
    HorizontalAlign="Left">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell Text="UserName"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Hometown"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Green Score"></asp:TableHeaderCell>
        
    </asp:TableHeaderRow>
</asp:Table>