<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="ActivEarth.Account.Profile" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to ActivEarth, <asp:Label runat="server" ID="lblUserName"></asp:Label>
    </h2>
    <p>
        <b>First Name:</b> <asp:Label runat="server" ID="lblFirstName"></asp:Label><br/>
        <b>Last Name:</b> <asp:Label runat="server" ID="lblLastName"></asp:Label><br/>
        <b>Email:</b> <asp:Label runat="server" ID="lblEmail"></asp:Label><br/>
        <b>Gender:</b> <asp:Label runat="server" ID="lblGender"></asp:Label><br/>
        <b>City, State:</b> <asp:Label runat="server" ID="lblCityState"></asp:Label><br/>
        <b>Age:</b> <asp:Label runat="server" ID="lblAge"></asp:Label><br/>
        <b>Height</b> <asp:Label runat="server" ID="lblHeight"></asp:Label> inches<br/>
        <b>Weight</b> <asp:Label runat="server" ID="lblWeight"></asp:Label> lbs<br/>
    </p>
    <br/><br/>
    <asp:HyperLink runat="server" Text="Edit Profile" NavigateUrl="~/Account/EditProfile.aspx"></asp:HyperLink><br/>
    <asp:HyperLink runat="server" Text="Change Password" NavigateUrl="~/Account/ChangePassword.aspx"></asp:HyperLink>
</asp:Content>
