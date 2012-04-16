<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.Master" AutoEventWireup="true" CodeBehind="DisplayContestPage.aspx.cs" Inherits="ActivEarth.Competition.Contests.DisplayContestPage" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register src="LeaderBoard.ascx" tagname="LeaderBoard" tagprefix="uc" %>

<%@ Register src="CountdownClock.ascx" tagname="CountdownClock" tagprefix="uc1" %>

<%@ Register src="ContestDisplayTable.ascx" tagname="ContestDisplayTable" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:Label ID="_ContestName" runat="server" Text="Contest Name" 
        Font-Names="Eras Bold ITC" Font-Overline="False" Font-Size="XX-Large" 
        ForeColor="Black"></asp:Label>
    <uc:LeaderBoard ID="_leaderBoard" runat="server" />    
    
    <uc2:ContestDisplayTable ID="ContestDisplayTable1" runat="server" />
    
</asp:Content>
