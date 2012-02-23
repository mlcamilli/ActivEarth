<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="ActivEarth.Account.EditProfile" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Profile for <asp:Label runat="server" ID="lblUserName"></asp:Label>
    </h2>
    <p>
        <b>First Name:</b> <asp:TextBox runat="server" ID="tbFirstName"></asp:TextBox><br/>
        <b>Last Name:</b> <asp:TextBox runat="server" ID="tbLastName"></asp:TextBox><br/>
        <b>Email:</b> <asp:TextBox runat="server" id="tbEmail"></asp:TextBox>
        <b>Gender:</b> <asp:DropDownList runat="server" ID="ddlGender">
                           <asp:ListItem Value="M">Male</asp:ListItem>
                           <asp:ListItem Value="F">Female</asp:ListItem>
                       </asp:DropDownList><br/>
        <b>City, State:</b> <asp:TextBox runat="server" ID="tbCity"></asp:TextBox>, <asp:TextBox runat="server" id="tbState" MaxLength="2" width="30"></asp:TextBox><br/>
        <b>Age:</b> <asp:TextBox runat="server" ID="tbAge"></asp:TextBox><br/>
        <b>Height</b> <asp:TextBox runat="server" ID="tbHeight"></asp:TextBox><br/>
        <b>Weight</b> <asp:TextBox runat="server" ID="tbWeight"></asp:TextBox><br/>
        <br /><br />
        <asp:Button runat="server" id="btnSaveChanges" onclick="SaveUserProfile" Text="Save Changes"/>
    </p>
</asp:Content>
