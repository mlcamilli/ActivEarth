<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayWeatherControl.ascx.cs" Inherits="ActivEarth.Objects.Profile.DisplayWeatherControl" %>

<asp:Panel ID="Panel1" runat="server" 
    style="position: relative; top: 0px; left: 2px; width: 150px;" 
    Height="150px">
    
    <asp:Label ID="Wind" runat="server" Text="Wind" 
        style="z-index: 1; left: 5px; top: 124px; position: absolute"></asp:Label>
        
    <asp:Label ID="Humidity" runat="server" Text="Humidity" 
        style="z-index: 1; left: 6px; top: 98px; position: absolute"></asp:Label>
        
    <asp:Label ID="Condition" runat="server" Text="Condition" 
        style="z-index: 1; left: 4px; top: 49px; position: absolute"></asp:Label>

    <asp:Image ID="ConditionImage" runat="server" Height="40px" 
        style="z-index: 1; left: 5px; top: 8px; position: absolute; height: 32px; width: 38px" 
        Width="40px" />
    <asp:Label ID="TempF" runat="server" 
        style="z-index: 1; left: 5px; top: 74px; position: absolute; width: 65px;" 
        Text="TempF"></asp:Label>
</asp:Panel>
