<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContestTimeGraph.ascx.cs" Inherits="ActivEarth.Competition.Contests.ContestTimeGraph" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<asp:Panel ID="_DisplayGraph" runat="server"   
    
    
    style="position: relative; top: 0px; left: 0px; height: 96px; width: 265px;">
    <asp:Label ID="_GraphTitle" runat="server" Text="Progress" 
        style="z-index: 1; left: 9px; top: 5px; position: absolute"></asp:Label>
    <asp:Panel ID="_DisplayProgress" runat="server" BorderColor="Black" 
        BorderStyle="Solid" Width="250px"  
        
        style="z-index: 1; left: 5px; top: 30px; position: absolute; height: 25px; width: 248px; bottom: 77px;">
         <eo:ProgressBar ID="_ContestProgress" runat="server" Width="258px" BackColor="#CCCCCC"
            style="z-index: 1; left: 0px; top: 5px; position: absolute; height: 15px; width: 248px" 
            IndicatorColor="ForestGreen"></eo:ProgressBar>
    </asp:Panel>

    <asp:Label ID="_StartLabel" runat="server" Text="Start" 
        style="z-index: 1; left: 7px; top: 65px; position: absolute"></asp:Label>
    <asp:Label ID="_EndLabel" runat="server" Text="Finish" 
        style="z-index: 1; left: 224px; top: 65px; position: absolute"></asp:Label>
</asp:Panel>