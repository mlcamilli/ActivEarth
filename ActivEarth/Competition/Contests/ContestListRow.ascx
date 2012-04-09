<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContestListRow.ascx.cs" Inherits="ActivEarth.Competition.Contests.ContestListRow" %>

<asp:Panel ID="_row" runat="server" BackColor="Green" Height="25px" 
    Width="400px" style="position: relative">

    <asp:Label ID="_contestName" runat="server" Text="Name"       
        style="z-index: 1; left: 7px; top: 5px; position: absolute; width: 229px;" 
        Font-Names="Eras Demi ITC" Font-Size="Small"></asp:Label>

</asp:Panel>