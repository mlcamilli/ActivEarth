<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.master" AutoEventWireup="true" CodeBehind="FindContestsPage.aspx.cs" Inherits="ActivEarth.Competition.Contests.FindContestsPage" %>

<%@ Register src="ContestDisplayTable.ascx" tagname="ContestDisplayTable" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="contentTitle">
        <asp:Label ID="lblTitle" Text="Find More Contests" runat="server" />
    </div> 
    <div align="center">
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" Width="150"><asp:Label ID="lblContestName" runat="server" Text="Contest Name" /></asp:TableCell><asp:TableCell><asp:TextBox ID="txtSearchText" runat="server" style="margin-bottom: 0px" /></asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell HorizontalAlign="Left"><asp:Label ID="lblExactMatch" runat="server" Text="Exact Match" /></asp:TableCell><asp:TableCell><asp:CheckBox ID="chkExactMatch" runat="server" Text="" Checked="false" /></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <asp:Button ID="btnFindContests" OnClick="ExecuteSearch" runat="server" Text="Find Contests" /> <br /><br />
        <uc1:ContestDisplayTable ID="SearchResults" Visible="false" runat="server" />
        <asp:Label ID="lblNoResultsFound" ForeColor="Red" Text="No Results Found" Visible="false" runat="server"/></div>

    </asp:Content>