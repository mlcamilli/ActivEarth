<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayBadgeControl.ascx.cs" Inherits="ActivEarth.Competition.Badges.DisplayBadgeControl" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<asp:Panel ID="_badgePanel" runat="server" Height="80px"  
    style="position: relative; z-index: 1; left: 10px; top: 15px; height: 80px; width: 400px; margin-top: 3px" Width="400px" 
    BackImageUrl="~/Images/Competition/Badges/Badge_Row.png">

    <asp:Label ID="_badgeName" runat="server" Text="Name" 
        style="z-index: 1; left: 81px; top: 3px; position: absolute; width: 222px; height: 17px;  text-align: left;" 
        Font-Names="Eras Bold ITC" Font-Bold="True" 
        Font-Size="Medium" ForeColor="Black" />

    <asp:Label ID="_activityPointsValue" runat="server" Text="1000"      
        style="z-index: 1; left: 330px; top: 4px; position: absolute; text-align: right; width: 41px;" 
        Font-Names="Eras Bold ITC" Font-Bold="False" Font-Size="Medium" 
        ForeColor="Black" />

    <asp:Image ID="_badgeImage" runat="server" Height="80px" 
        style="z-index: 1; left: 0px; top: 0px; position: absolute" Width="80px" 
        ImageUrl="~/Images/Competition/Badges/Steps/Bronze.png"/>

    <eo:ProgressBar ID="_badgeProgressBar" runat="server"              
        style="z-index: 1; left: 82px; top: 55px; position: absolute; height: 36px; width: 314px" 
        BackgroundImage="~/Images/Competition/ProgressBar/ProgressBarBackground.png" 
        BackgroundImageLeft="~/Images/Competition/ProgressBar/ProgressBarLeft.png" 
        BackgroundImageRight="~/Images/Competition/ProgressBar/ProgressBarRight.png" 
        IndicatorImage="~/Images/Competition/ProgressBar/ProgressBarIndicator.png">
    </eo:ProgressBar>

    <asp:Label ID="_badgeProgressNumerical" runat="server" Text="0 / 0" 
        style="z-index: 1; left: 93px; top: 32px; position: absolute; text-align: right; width: 299px;" 
        Font-Names="Eras Bold ITC" Font-Bold="False" Font-Size="Medium" 
        ForeColor="Black" />

    <asp:Image ID="_activityScoreImage" runat="server" Height="20px" 
        style="z-index: 1; left: 374px; top: 4px; position: absolute" Width="20px" 
        ImageUrl="~/Images/Competition/Activity_Score.png"/>


</asp:Panel>

    

