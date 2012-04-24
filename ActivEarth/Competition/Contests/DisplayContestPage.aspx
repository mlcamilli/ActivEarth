﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.Master" AutoEventWireup="true" CodeBehind="DisplayContestPage.aspx.cs" Inherits="ActivEarth.Competition.Contests.DisplayContestPage" %>

<%@ Register src="LeaderBoard.ascx" tagname="LeaderBoard" tagprefix="uc" %>
<%@ Register src="ContestGoalGraph.ascx" tagname="ContestGoalGraph" tagprefix="uc" %>
<%@ Register src="ContestTimeGraph.ascx" tagname="ContestTimeGraph" tagprefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
    <div align="right">
        <asp:Panel ID="ActivtyScorePanel" runat="server" style="position: relative">
            <asp:Label ID="ContestActivityScore" runat="server" Text="100" 
                style="z-index: 1; left: 249px; top: 2px; position: absolute; width: 289px"></asp:Label>
            <asp:Image ID="_activityScoreImage" runat="server" Height="20px" Width="20px" 
                ImageUrl="~/Images/Competition/Activity_Score.png"/>
        </asp:Panel>
    </div>
    <p>
        <h2><asp:Label ID="ContestName" runat="server" Text="Contest Name"></asp:Label></h2>
        <asp:Label ID="ContestDescription" runat="server" Text="Contest Description"></asp:Label>
    </p>
    <div align="left">
        <asp:Table ID="ContestStatusTable"  runat="server" CellPadding="0" 
            CellSpacing="0">
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell>
                    <uc:ContestGoalGraph ID="GoalGraph" runat="server" Visible="False" />
                    <uc:ContestTimeGraph ID="TimeGraph" runat="server" Visible="False" />
                </asp:TableCell>               
                <asp:TableCell>
                    <uc:LeaderBoard ID="ContestLeaderBoard" runat="server" Visible="False"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <asp:Button ID="btnJoinContest" Text="Join Contest" OnClick="JoinContest" runat="server" Visible="false" />
    </div> 
    
</asp:Content>
