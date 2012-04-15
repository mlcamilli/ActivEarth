<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayChallengeControl.ascx.cs" Inherits="ActivEarth.Competition.Challenges.DisplayChallengeControl" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<asp:Panel ID="_challengePanel" runat="server" Height="80px"  
    style="position: relative; z-index: 1; left: 10px; top: 15px; height: 120px; width: 480px; margin-top: 3px" Width="400px" 
    BackImageUrl="~/Images/Competition/Challenges/Challenges_Row.png">

    <asp:Label ID="_challengeName" runat="server" Text="Name" 
        style="z-index: 1; left: 86px; top: 5px; position: absolute; width: 295px; height: 17px;  text-align: left;" 
        Font-Names="Eras Bold ITC" Font-Bold="True" 
        Font-Size="Medium" ForeColor="Black" />

    <asp:Label ID="_activityPointsValue" runat="server" Text="1000"      
        style="z-index: 1; left: 408px; top: 4px; position: absolute; text-align: right; width: 41px;" 
        Font-Names="Eras Bold ITC" Font-Bold="False" Font-Size="Medium" 
        ForeColor="Black" />

    <asp:Image ID="_challengeImage" runat="server" Height="80px" 
        style="z-index: 1; left: 0px; top: 20px; position: absolute" Width="80px" 
        
        ImageUrl="~/Images/Competition/Challenges/Challenges_Template/Monthly.png"/>

    <eo:ProgressBar ID="_challengeProgressBar" runat="server"        
        style="z-index: 1; left: 87px; top: 89px; position: absolute; height: 36px; width: 385px" 
        BackgroundImage="~/Images/Competition/ProgressBar/ProgressBarBackground.png" 
        BackgroundImageLeft="~/Images/Competition/ProgressBar/ProgressBarLeft.png" 
        BackgroundImageRight="~/Images/Competition/ProgressBar/ProgressBarRight.png" 
        IndicatorImage="~/Images/Competition/ProgressBar/ProgressBarIndicator.png">
    </eo:ProgressBar>

    <asp:Label ID="_challengeProgressNumerical" runat="server" Text="0 / 0" 
        style="z-index: 1; left: 95px; top: 70px; position: absolute; text-align: right; width: 375px;" 
        Font-Names="Eras Bold ITC" Font-Bold="False" Font-Size="Medium" 
        ForeColor="Black" />

    <asp:Label ID="_challengeDescription" runat="server" Text="Description" 
        style="z-index: 1; left: 88px; top: 31px; position: absolute; text-align: left; width: 385px;" 
        Font-Names="Eras Demi ITC" Font-Bold="False" Font-Size="Small" 
        ForeColor="Black" />

    <asp:Image ID="_activityScoreImage" runat="server" Height="20px" 
        style="z-index: 1; left: 454px; top: 4px; position: absolute" Width="20px" 
        ImageUrl="~/Images/Competition/Activity_Score.png"/>

</asp:Panel>