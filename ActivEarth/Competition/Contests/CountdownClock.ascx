<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountdownClock.ascx.cs" Inherits="ActivEarth.Competition.Contests.CountdownClock" %>
<span id="timerLabel" runat="server"></span>

<script type="text/javascript">

    function countdown() {
        seconds = document.getElementById("timerLabel").innerHTML;
        if (seconds > 0) {
            document.getElementById("timerLabel").innerHTML = seconds - 1;
            setTimeout("countdown()", 1000);
        }
    }

    setTimeout("countdown()", 1000);

</script>

<asp:Panel ID="_ClockDisplay" runat="server" Width="300px" Height="275" 
    BorderColor="Black" BorderStyle="Solid" style="position: relative">
    <asp:Label ID="_DaysLeft" runat="server" Text="300" Font-Names="Eras Bold ITC" 
        Font-Size="XX-Large" 
        
        style="z-index: 1; left: 11px; top: 76px; position: absolute; text-align: right"></asp:Label>
        <asp:Label ID="_HoursLeft" runat="server" Text="300" Font-Names="Eras Bold ITC" 
        Font-Size="XX-Large" 
        
        style="z-index: 1; left: 90px; top: 76px; position: absolute; text-align: right"></asp:Label>
        <asp:Label ID="_MinutesLeft" runat="server" Text="300" Font-Names="Eras Bold ITC" 
        Font-Size="XX-Large" 
        
        style="z-index: 1; left: 162px; top: 76px; position: absolute; text-align: right"></asp:Label>
        <asp:Label ID="_SecondsLeft" runat="server" Text="300" Font-Names="Eras Bold ITC" 
        Font-Size="XX-Large" 
        
        style="z-index: 1; left: 234px; top: 76px; position: absolute; text-align: right"></asp:Label>
</asp:Panel>

