<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeaderBoardRow.ascx.cs" Inherits="ActivEarth.Competition.Contests.LeaderBoardRow" %>

<asp:Panel ID="_row" runat="server" BackColor="Green" Height="25px" 
    Width="300px" style="position: relative">

    <asp:Label ID="_position" runat="server" Text="."        
        style="z-index: 1; left: 2px; top: 5px; position: absolute; text-align:right; width: 27px;"></asp:Label>

    <asp:Label ID="_teamName" runat="server" Text="Name"       
        style="z-index: 1; left: 31px; top: 5px; position: absolute; width: 150px;"></asp:Label>

    <asp:Label ID="_currentScore" runat="server" Text="10000"      
        style="z-index: 1; left: 193px; top: 5px; position: absolute; text-align:right; width: 100px;"></asp:Label>

</asp:Panel>

