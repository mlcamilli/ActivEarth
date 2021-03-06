﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.master" AutoEventWireup="true" CodeBehind="ContestHomePage.aspx.cs" Inherits="ActivEarth.Competition.Contests.ContestHomePage" %>

<%@ Register src="ContestDisplayTable.ascx" tagname="ContestDisplayTable" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
    <div align="center">
        <asp:Table ID="btnPanel"  runat="server" style="position: relative">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Button ID="btnCreateContest" runat="server" Text="Create Contest" OnClick="OpenCreateContestPage" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="btnSearchContests" runat="server" Text="Find Contests" OnClick="OpenFindContestsPage"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <br />
    <div class="contentTitle" align="center">
    Current Contests  
    </div> 
    <div align="center">
        <uc1:ContestDisplayTable ID="displayCurrentContests" runat="server" />
    </div> 

</asp:Content>


