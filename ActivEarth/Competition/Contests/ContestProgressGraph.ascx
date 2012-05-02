<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContestProgressGraph.ascx.cs" Inherits="ActivEarth.Competition.Contests.ContestTimeGraph" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<asp:Panel ID="_DisplayGraph" runat="server"    
    
    style="position: relative; top: 0px; left: 0px; height: 67px; width: 265px;">

    <asp:Panel ID="_DisplayProgress" runat="server" BorderColor="Black" 
        BorderStyle="Solid" Width="250px"  
        style="z-index: 1; left: 5px; top: 10px; position: absolute; height: 25px; width: 248px; bottom: 77px;">
         <eo:ProgressBar ID="_ContestProgress" runat="server" Width="258px" BackColor="#CCCCCC"
            style="z-index: 1; left: 0px; top: 5px; position: absolute; height: 15px; width: 248px" 
            IndicatorColor="ForestGreen"></eo:ProgressBar>
    </asp:Panel>

    <asp:Label ID="_StartLabel" runat="server" Text="Start" 
        style="z-index: 1; left: 7px; top: 45px; position: absolute"></asp:Label>
    <asp:Label ID="_EndLabel" runat="server" Text="Finish" 
        style="z-index: 1; left: 61px; top: 45px; position: absolute; width: 198px; text-align: right"></asp:Label>
</asp:Panel>