<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeaderBoardRow.ascx.cs" Inherits="ActivEarth.Competition.Contests.LeaderBoardRow" %>

<asp:Panel ID="Row" runat="server" Height="25px" 
    Width="300px" style="position: relative">
    
    <asp:Image ID="BracketImage" runat="server" Height="25px" ImageAlign="Middle" 
        style="z-index: 1; left: 0px; top: 0px; position: absolute" Width="25px" />

    <asp:Label ID="TeamName" runat="server" Text="Name"        
        
        style="z-index: 1; left: 30px; top: 5px; position: absolute; width: 162px;"></asp:Label>

    <asp:Label ID="CurrentScore" runat="server" Text="10000"         
        
        style="z-index: 1; left: 204px; top: 5px; position: absolute; width: 89px; text-align: right;"></asp:Label>

</asp:Panel>

