<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.Master" AutoEventWireup="true" CodeBehind="DisplayChallengesPage.aspx.cs" Inherits="ActivEarth.Competition.Challenges.DisplayChallengesPage" %>
<%@ Register src="DisplayChallengesControl.ascx" tagname="DisplayChallengesControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentTitle">
        Daily Challenges 
        <uc1:DisplayChallengesControl ID="_displayDailyChallenges" runat="server" />
        Weekly Challenges
        <uc1:DisplayChallengesControl ID="_displayWeeklyChallenges" runat="server" />
        Monthly Challenges
        <uc1:DisplayChallengesControl ID="_displayMonthlyChallenges" runat="server" />
        Permanent Challenges
        <uc1:DisplayChallengesControl ID="_displayPersistentChallenges" runat="server" />
    </div>
</asp:Content>
