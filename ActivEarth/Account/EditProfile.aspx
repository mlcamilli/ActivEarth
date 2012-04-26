<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="ActivEarth.Account.EditProfile" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Profile for <asp:Label runat="server" ID="lblUserName"></asp:Label>
    </h2>
    <table>
        <tbody>
            <tr>
                <td><b>First Name:</b></td>
                <td><asp:TextBox runat="server" ID="tbFirstName"></asp:TextBox></td>
            </tr>
            <tr>
                <td><b>Last Name:</b></td>
                <td><asp:TextBox runat="server" ID="tbLastName"></asp:TextBox></td>
            </tr>
            <tr>
                <td><b>Email:</b></td>
                <td><asp:TextBox runat="server" id="tbEmail"></asp:TextBox></td>
            </tr>
            <tr>
                <td><b>Gender:</b></td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlGender">
                        <asp:ListItem Value="M">Male</asp:ListItem>
                        <asp:ListItem Value="F">Female</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td><b>City, State:</b></td>
                <td><asp:TextBox runat="server" ID="tbCity"></asp:TextBox>, <asp:TextBox runat="server" id="tbState" MaxLength="2" width="30"></asp:TextBox></td>
            </tr>
            <tr>
                <td><b>Age:</b></td>
                <td><asp:TextBox runat="server" ID="tbAge"></asp:TextBox></td>
            </tr>
            <tr>
                <td><b>Height</b></td>
                <td><asp:TextBox runat="server" ID="tbHeight"></asp:TextBox></td>
            </tr>
            <tr>
                <td><b>Weight</b></td>
                <td><asp:TextBox runat="server" ID="tbWeight"></asp:TextBox></td>
            </tr>
            <tr>
                <td><strong><asp:Label ID="pictureLabel" runat="server" Text="Picture"></asp:Label></strong></td>
                <td><asp:FileUpload ID="pictureFile" runat="server" /></td>
            </tr>
            <tr>
                <td></td>
                <td><asp:Button runat="server" id="btnSaveChanges" onclick="SaveUserProfile" Text="Save Changes"/></td>
            </tr>
        <br /><br />
        
    </table>
</asp:Content>
