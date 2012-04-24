<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContestGoalGraph.ascx.cs" Inherits="ActivEarth.Competition.Contests.ContestGraph" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<asp:Panel ID="_DisplayGraph" runat="server"   
    
    style="position: relative; top: 0px; left: 0px; height: 217px; width: 265px;">
    <asp:Label ID="_GraphTitle" runat="server" Text="Progress" 
        style="z-index: 1; left: 9px; top: 5px; position: absolute"></asp:Label>
    <asp:Panel ID="_DisplayComparision" runat="server" BorderColor="Black" 
        BorderStyle="Solid" Width="250px" 
        style="z-index: 1; left: 5px; top: 30px; position: absolute; height: 154px; width: 248px" 
        BorderWidth="1px">
        <asp:Label ID="_FirstPlaceTeamName" runat="server" Text="Label" 
            style="z-index: 1; left: 3px; top: 0px; position: absolute">
         </asp:Label>
         <eo:ProgressBar ID="_FirstPlaceProgress" runat="server" Width="258px"   
            style="z-index: 1; left: 0px; top: 15px; position: absolute; height: 15px; width: 248px" 
            IndicatorColor="ForestGreen" BackColor="#CCCCCC"></eo:ProgressBar>
         <asp:Label ID="_SecondPlaceTeamName" runat="server" Text="Label" 
            style="z-index: 1; left: 4px; top: 37px; position: absolute">
         </asp:Label>
         <eo:ProgressBar ID="_SecondPlaceProgress" runat="server" Width="258px"   
            style="z-index: 1; left: 0px; top: 53px; position: absolute; height: 15px; width: 248px"  
            IndicatorColor="ForestGreen" BackColor="#CCCCCC"></eo:ProgressBar>
         <asp:Label ID="_ThirdPlaceTeamName" runat="server" Text="Label" 
            style="z-index: 1; left: 3px; top: 75px; position: absolute">
         </asp:Label>
         <eo:ProgressBar ID="_ThirdPlaceProgress" runat="server" Width="258px"   
            style="z-index: 1; left: 0px; top: 91px; position: absolute; height: 15px; width: 248px"  
            IndicatorColor="ForestGreen" BackColor="#CCCCCC"></eo:ProgressBar>
         <asp:Label ID="_CurrentUserName" runat="server" Text="Label" 
            style="z-index: 1; left: 2px; top: 115px; position: absolute">
         </asp:Label>
         <eo:ProgressBar ID="_CurrentUserProgress" runat="server" Width="258px"   
            style="z-index: 1; left: 0px; top: 131px; position: absolute; height: 15px; width: 248px"  
            IndicatorColor="ForestGreen" BackColor="#CCCCCC"></eo:ProgressBar>
    </asp:Panel>
    <asp:Label ID="_GoalLabel" runat="server" Text="100"  
        
        
        style="z-index: 1; left: 63px; top: 192px; position: absolute; text-align: right; width: 199px;">
    </asp:Label>
    <asp:Label ID="_StartLabel" runat="server" 
        style="z-index: 1; left: 5px; top: 192px; position: absolute; width: 47px; text-align: left" 
        Text="0">
    </asp:Label>
</asp:Panel>