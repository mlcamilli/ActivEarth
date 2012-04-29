<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContestRewardsTable.ascx.cs" Inherits="ActivEarth.Competition.Contests.ActivityPointsTable" %>

<asp:Table ID="PointsTable"  runat="server" CellPadding="0" CellSpacing="0">
    <asp:TableRow VerticalAlign="Middle">
        <asp:TableCell>
            <asp:Panel ID="DiamondBracket" runat="server" style="position: relative;" Height="30">
                <asp:Image ID="DiamondBracketImage" runat="server" Height="25px" Width="25px" 
                    ImageUrl="~/Images/Competition/Contests/DiamondBracket.png" ImageAlign="Middle" style="margin-right: 3px"/>
                <asp:Label ID="DiamondBracketReward" runat="server" Text="100"></asp:Label>
                <asp:Image ID="DiamondActivityScoreImage" runat="server" Height="20px" Width="20px" 
                    ImageUrl="~/Images/Competition/Activity_Score.png" ImageAlign="Middle" style="margin-right: 20px"/>
            </asp:Panel>
        </asp:TableCell>               
        <asp:TableCell>
            <asp:Panel ID="PlatinumBracket" runat="server" style="position: relative">
                 <asp:Image ID="PlatinumBracketImage" runat="server" Height="25px" Width="25px" 
                    ImageUrl="~/Images/Competition/Contests/PlatinumBracket.png" ImageAlign="Middle" style="margin-right: 3px"/>
                <asp:Label ID="PlatinumBracketReward" runat="server" Text="100"></asp:Label>
                <asp:Image ID="PlatinumActivityScoreImage" runat="server" Height="20px" Width="20px" 
                    ImageUrl="~/Images/Competition/Activity_Score.png" ImageAlign="Middle" style="margin-right: 20px"/>
            </asp:Panel>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Panel ID="GoldBracket" runat="server" style="position: relative">
                 <asp:Image ID="GoldBracketImage" runat="server" Height="25px" Width="25px" 
                    ImageUrl="~/Images/Competition/Contests/GoldBracket.png" ImageAlign="Middle" style="margin-right: 3px"/>
                <asp:Label ID="GoldBracketReward" runat="server" Text="100"></asp:Label>
                <asp:Image ID="GoldActivityScoreImage" runat="server" Height="20px" Width="20px" 
                    ImageUrl="~/Images/Competition/Activity_Score.png" ImageAlign="Middle" style="margin-right: 20px"/>
            </asp:Panel>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Panel ID="SilverBracket" runat="server" style="position: relative">
                 <asp:Image ID="SilverBracketImage" runat="server" Height="25px" Width="25px" 
                    ImageUrl="~/Images/Competition/Contests/SilverBracket.png" ImageAlign="Middle" style="margin-right: 3px"/>
                <asp:Label ID="SilverBracketReward" runat="server" Text="100"></asp:Label>
                <asp:Image ID="SiverActivityScoreImage" runat="server" Height="20px" Width="20px" 
                    ImageUrl="~/Images/Competition/Activity_Score.png" ImageAlign="Middle" style="margin-right: 20px"/>
            </asp:Panel>
        </asp:TableCell>               
        <asp:TableCell>
            <asp:Panel ID="BronzeBracket" runat="server" style="position: relative">
                 <asp:Image ID="BronzeBracketImage" runat="server" Height="25px" Width="25px" 
                    ImageUrl="~/Images/Competition/Contests/BronzeBracket.png" ImageAlign="Middle" style="margin-right: 3px"/>
                <asp:Label ID="BronzeBracketReward" runat="server" Text="100"></asp:Label>
                <asp:Image ID="BronzeActivityScoreImage" runat="server" Height="20px" Width="20px" 
                    ImageUrl="~/Images/Competition/Activity_Score.png" ImageAlign="Middle" style="margin-right: 20px"/>
            </asp:Panel>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Panel ID="NoneBracket" runat="server" style="position: relative">
                 <asp:Image ID="NoneBracketImage" runat="server" Height="25px" Width="25px" 
                    ImageUrl="~/Images/Competition/Contests/NoneBracket.png" ImageAlign="Middle" style="margin-right: 3px"/>
                <asp:Label ID="NoneBracketReward" runat="server" Text="0"></asp:Label>
                <asp:Image ID="NoneActivityScoreImage" runat="server" Height="20px" Width="20px" 
                    ImageUrl="~/Images/Competition/Activity_Score.png" ImageAlign="Middle" />
            </asp:Panel>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>