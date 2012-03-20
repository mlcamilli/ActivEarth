<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BMI.aspx.cs" Inherits="ActivEarth.Fitness.BMI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Your BMI
    </h2>
    <asp:Panel runat="server" ID="pnlBMI">
    <p>
        Your BMI is <b><asp:Label runat="server" ID="lblBMI"></asp:Label></b> which means you are <b><asp:Label runat="server" ID="lblBMIResult"></asp:Label></b>.
    </p>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlNotEnoughData">
        <br/>
        <asp:Label runat="server" Text="You have not updated your profile with all of the necessary fields. Please do so "></asp:Label><asp:HyperLink runat="server" Text="here." NavigateUrl="~/Account/EditProfile.aspx"></asp:HyperLink>
    </asp:Panel>

</asp:Content>
