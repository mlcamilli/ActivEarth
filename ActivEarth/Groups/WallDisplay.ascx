<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="WallDisplay.ascx.cs" Inherits="ActivEarth.Groups.WallDisplay" %>

<asp:Table ID="_wall" runat="server" BorderStyle="Solid" 
    BorderColor="Black" ForeColor="Black" Font-Names="Georgia" 
    CellPadding="3" CellSpacing="3" GridLines="Both" BorderWidth="2px" 
    HorizontalAlign="Left">
    <asp:TableHeaderRow>
        <asp:TableHeaderCell Text="Poster Name"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Title"></asp:TableHeaderCell>
        <asp:TableHeaderCell Text="Text"></asp:TableHeaderCell>
        
    </asp:TableHeaderRow>
</asp:Table>