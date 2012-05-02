<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.Master" AutoEventWireup="true" CodeBehind="DisplayContestPage.aspx.cs" Inherits="ActivEarth.Competition.Contests.DisplayContestPage" %>

<%@ Register src="LeaderBoard.ascx" tagname="LeaderBoard" tagprefix="uc" %>
<%@ Register src="TeamDisplayTable.ascx" tagname="TeamDisplayTable" tagprefix="uc" %>
<%@ Register src="ContestProgressGraph.ascx" tagname="ContestProgressGraph" tagprefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <asp:Label ID="ContestName" runat="server" Text="Contest Name"></asp:Label>
    </h2>
    <p>
        <asp:Label ID="ContestDescription" runat="server" Text="Contest Description"></asp:Label>
    </p>

    <div align="center">
        <asp:Panel ID="ContestSignUpPanel" runat="server" Visible="false">
            <br />
            <asp:Label runat="server" ID="NoTeamsMessage" Visible="false"></asp:Label>
            <uc:TeamDisplayTable runat="server" ID="CurrentTeamsSignedUp" Visible="true"/>
            <br />
            <asp:Label runat="server" ID="SignUpErrorMessage" Visible="false"></asp:Label>
            <asp:Table ID="ContestSignUpControls"  runat="server" CellPadding="3" CellSpacing="3">
                <asp:TableRow VerticalAlign="Middle">
                    <asp:TableCell>
                        <asp:Button ID="btnJoinContest" Text="Join Contest" OnClick="JoinContest" runat="server" Visible="false" />
                        <asp:Button ID="btnLeaveContest" Text="Leave Contest" OnClick="LeaveContest" runat="server" Visible="false" />
                    </asp:TableCell>               
                    <asp:TableCell>
                        <asp:DropDownList ID="GroupSelection" runat="server" Visible="false" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>

    <div align="center">
        <asp:Table ID="ContestStatsTable"  runat="server" CellPadding="3" CellSpacing="3" Visible="false">
            <asp:TableHeaderRow BackColor="White">
                <asp:TableHeaderCell Text="Teams Competing"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Contest Progress"></asp:TableHeaderCell>
                <asp:TableHeaderCell Text="Total Reward"></asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow VerticalAlign="Top">
                <asp:TableCell HorizontalAlign="Center">
                    <asp:Label ID="TotalTeamsLabel" runat="server" Font-Size="Large" />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Center">
                    <uc:ContestProgressGraph ID="ProgressGraph" runat="server" />
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Center">
                    <asp:Label ID="TotalRewardLabel" runat="server" style="margin-right: 5px" />
                    <asp:Image ID="ActivityScoreImage" runat="server" Height="20px" Width="20px" ImageAlign="Middle" ImageUrl="~/Images/Competition/Activity_Score.png" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        
        <uc:LeaderBoard ID="ContestLeaderBoard" runat="server" Visible="False"/>
    </div>
    
</asp:Content>
