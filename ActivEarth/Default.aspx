<%@ Page Title="ActivEarth Home" Language="C#" MasterPageFile="~/SiteNotLoggedIn.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ActivEarth._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to ActivEarth <asp:Label runat="server" ID="lblMainUserName"></asp:Label>!
    </h2>
    <h3>
        This site is currently under construction and will be updated soon. 
    </h3>
    <asp:HyperLink ID="hlAccessProfile" runat="server" Text="Access Your Profile" NavigateUrl="~/Account/Profile.aspx"></asp:HyperLink><br/>
    <h1><asp:HyperLink ID="hlAbout" runat="server" Text="About" NavigateUrl="~/About.aspx"></asp:HyperLink><br/></h1>
    
</asp:Content>
