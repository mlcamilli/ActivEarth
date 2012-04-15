<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayChallengesPage.aspx.cs" Inherits="ActivEarth.Competition.Challenges.DisplayChallengesPage" %>
<%@ Register src="DisplayChallengesControl.ascx" tagname="DisplayChallengesControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div align="center">
        <asp:Label ID="_dailyChallengeLabel" runat="server" Text="Daily Challenges" 
        Font-Names="Eras Bold ITC" Font-Bold="True" 
        Font-Size="Large" ForeColor="Black" />
        <uc1:DisplayChallengesControl ID="_displayDailyChallenges" runat="server" />
        <asp:Label ID="_weeklyChallengeLabel" runat="server" Text="Weekly Challenges"
        Font-Names="Eras Bold ITC" Font-Bold="True" 
        Font-Size="Large" ForeColor="Black" />
        <uc1:DisplayChallengesControl ID="_displayWeeklyChallenges" runat="server" />
        <asp:Label ID="_monthlyChallengesLabel" runat="server" Text="Monthly Challenges"
        Font-Names="Eras Bold ITC" Font-Bold="True" 
        Font-Size="Large" ForeColor="Black" />
        <uc1:DisplayChallengesControl ID="_displayMonthlyChallenges" runat="server" />
        <asp:Label ID="_persistentChallenges" runat="server" Text="Permanent Challenges"
        Font-Names="Eras Bold ITC" Font-Bold="True" 
        Font-Size="Large" ForeColor="Black" />
        <uc1:DisplayChallengesControl ID="_displayPersistentChallenges" runat="server" />
    </div>
</asp:Content>
