<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Tabs.master" AutoEventWireup="true"
    CodeBehind="ChangePasswordSuccess.aspx.cs" Inherits="ActivEarth.Account.ChangePasswordSuccess" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Change Password
    </h2>
    <p>
        Your password has been changed successfully.
    </p>
    
    <asp:HyperLink runat="server" Text="Return to Profile" NavigateUrl="~/Account/Profile.aspx"></asp:HyperLink>
</asp:Content>
