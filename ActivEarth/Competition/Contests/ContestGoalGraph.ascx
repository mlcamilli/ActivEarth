<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContestGoalGraph.ascx.cs" Inherits="ActivEarth.Competition.Contests.ContestGraph" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<asp:Panel ID="_DisplayGraph" runat="server"   
    style="position: relative; top: 0px; left: 0px; height: 291px; width: 265px;">
    <asp:Label ID="_GraphTitle" runat="server" Text="Progress" 
        style="z-index: 1; left: 9px; top: 5px; position: absolute"></asp:Label>
    <asp:Panel ID="_DisplayComparision" runat="server" BorderColor="Black" 
        BorderStyle="Solid" Width="250px" style="z-index: 1; left: 5px; top: 30px; position: absolute; height: 230px; width: 248px">
        <asp:Label ID="_FirstPlaceTeamName" runat="server" Text="Label" 
            style="z-index: 1; left: 3px; top: 0px; position: absolute">
         </asp:Label>
         <eo:ProgressBar ID="_FirstPlaceProgress" runat="server" Width="258px"   
            style="z-index: 1; left: 0px; top: 15px; position: absolute; height: 30px; width: 248px" 
            IndicatorColor="ForestGreen"></eo:ProgressBar>
         <asp:Label ID="_SecondPlaceTeamName" runat="server" Text="Label" 
            style="z-index: 1; left: 3px; top: 56px; position: absolute">
         </asp:Label>
         <eo:ProgressBar ID="_SecondPlaceProgress" runat="server" Width="258px"   
            style="z-index: 1; left: 0px; top: 70px; position: absolute; height: 30px; width: 248px"  
            IndicatorColor="ForestGreen"></eo:ProgressBar>
         <asp:Label ID="_ThirdPlaceTeamName" runat="server" Text="Label" 
            style="z-index: 1; left: 3px; top: 114px; position: absolute">
         </asp:Label>
         <eo:ProgressBar ID="_ThirdPlaceProgress" runat="server" Width="258px"   
            style="z-index: 1; left: 0px; top: 130px; position: absolute; height: 30px; width: 248px"  
            IndicatorColor="ForestGreen"></eo:ProgressBar>
         <asp:Label ID="_CurrentUserName" runat="server" Text="Label" 
            style="z-index: 1; left: 3px; top: 175px; position: absolute">
         </asp:Label>
         <eo:ProgressBar ID="_CurrentUserProgress" runat="server" Width="258px"   
            style="z-index: 1; left: 0px; top: 190px; position: absolute; height: 30px; width: 248px"  
            IndicatorColor="ForestGreen"></eo:ProgressBar>
    </asp:Panel>
    <asp:Label ID="_GoalLabel" runat="server" Text="100"  
        style="z-index: 1; left: 238px; top: 267px; position: absolute; text-align: right">
    </asp:Label>
    <asp:Label ID="_StartLabel" runat="server" 
        style="z-index: 1; left: 5px; top: 267px; position: absolute; width: 47px; text-align: left" 
        Text="0">
    </asp:Label>
</asp:Panel>