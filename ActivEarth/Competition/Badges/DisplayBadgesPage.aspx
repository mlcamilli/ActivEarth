<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayBadgesPage.aspx.cs" Inherits="ActivEarth.Competition.Badges.DisplayBadgesPage" %>
<%@ Register src="DisplayBadgesControl.ascx" tagname="DisplayBadgesControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     
    <div align="center">
    <uc1:DisplayBadgesControl ID="_displayBadgesControl" runat="server"/>
    </div>
    
</asp:Content>
