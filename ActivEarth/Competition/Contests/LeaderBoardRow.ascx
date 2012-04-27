<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeaderBoardRow.ascx.cs" Inherits="ActivEarth.Competition.Contests.LeaderBoardRow" %>

<asp:Panel ID="_row" runat="server" Height="25px" 
    Width="300px" style="position: relative">

    <asp:Label ID="_teamName" runat="server" Text="Name"        
        style="z-index: 1; left: 7px; top: 5px; position: absolute; width: 174px;"></asp:Label>

    <asp:Label ID="_currentScore" runat="server" Text="10000"         
        style="z-index: 1; left: 193px; top: 5px; position: absolute; width: 100px; text-align: right;"></asp:Label>

</asp:Panel>

