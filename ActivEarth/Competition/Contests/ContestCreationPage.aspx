<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.master" AutoEventWireup="true" CodeBehind="ContestCreationPage.aspx.cs" Inherits="ActivEarth.Competition.Contests.ContestCreationPage" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ToolKitScript" />   
    <h2>
        Create a Contest
    </h2>
    <p>
        Use the form below to create a new contest.
    </p>
    <span class="failureNotification">
                    <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </span>
                    <asp:ValidationSummary ID="CreateContestValidationSummary" runat="server" CssClass="failureNotification" 
                         ValidationGroup="CreateContestValidationGroup"/>
    <div class="contestInfo">
        <fieldset class="createContest">
            <legend>Contest Information</legend>
                <p>
                    <asp:Label ID="ContestNameLabel" runat="server" AssociatedControlID="txbContestName">Contest Name:</asp:Label>
                    <asp:TextBox ID="txbContestName" runat="server" CssClass="textEntry"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ContestNameRequired" runat="server" ControlToValidate="txbContestName" 
                        CssClass="failureNotification" ErrorMessage="Contest Name is required." ToolTip="Contest Name is required." 
                        ValidationGroup="CreateContestValidationGroup">*</asp:RequiredFieldValidator> 
                </p>
                <p>
                    <asp:Label ID="contestDescriptionLabel" runat="server" AssociatedControlID="txbContestDescription">Contest Description:</asp:Label>
                    <asp:TextBox ID="txbContestDescription" runat="server" CssClass="textMultilineEntry" 
                        TextMode="MultiLine" Width="320px"></asp:TextBox>
                </p>
                <p>
                    <asp:Label ID="contestTypeLabel" runat="server" Text="Contest Type:"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlContestType">
                        <asp:ListItem  Text="Individual" Value="Individual"></asp:ListItem>
                        <asp:ListItem  Text="Group" Value="Group"></asp:ListItem>
                    </asp:DropDownList>
                </p>
                <p>
                    <asp:Label ID="statisticMeasuredLabel" runat="server" Text="Statistic Measured:"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlStatisticMeasured">
                    </asp:DropDownList>
                </p>
                <p>
                    <asp:Label ID="contestStartDateLabel" runat="server" Text="Contest Start Date:"></asp:Label>
                    <asp:TextBox ID="txbContestStartDate" runat="server" Enabled="True" ></asp:TextBox>
                    <asp:ImageButton runat="Server" ID="displayStartDateCalender" 
                        AlternateText="Display Calender" 
                        ImageUrl="~/Images/Competition/Misc/Calender.png" ImageAlign="AbsMiddle" /> 
                    <ajaxToolkit:CalendarExtender ID="contestStartDateCalender" runat="server" PopupButtonID="displayStartDateCalender" TargetControlID="txbContestStartDate" />
                    <asp:RequiredFieldValidator ID="contestStartRequired" runat="server" ControlToValidate="txbContestStartDate" 
                        CssClass="failureNotification" ErrorMessage="Contest Start Date is required." ToolTip="Contest Start Date is required." 
                        ValidationGroup="CreateContestValidationGroup">*</asp:RequiredFieldValidator>
                </p>
                 <p>
                    <asp:Label ID="contestModeLabel" runat="server" Text="Contest Mode:"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlContestMode" AutoPostBack="true" 
                         onselectedindexchanged="ddlContestMode_SelectedIndexChanged">
                        <asp:ListItem  Text="Time Based" Value="Time"></asp:ListItem>
                        <asp:ListItem  Text="Goal Based" Value="Goal"></asp:ListItem>
                    </asp:DropDownList>
                </p>
                <asp:Panel ID="contestModeTimePanel" runat="server" Visible="True">
                    <p>
                        <asp:Label ID="contestEndDateLabel" runat="server" Text="Contest End Date:"></asp:Label>
                        <asp:TextBox ID="txbContestEndDate" runat="server" Enabled="True"></asp:TextBox>
                        <asp:ImageButton runat="Server" ID="displayEndDateCalender" AlternateText="Display Calender" 
                            ImageUrl="~/Images/Competition/Misc/Calender.png" ImageAlign="AbsMiddle" /> 
                        <ajaxToolkit:CalendarExtender ID="contestEndDateCalender" runat="server" PopupButtonID="displayEndDateCalender" TargetControlID="txbContestEndDate" />
                    </p>
                </asp:Panel>
                <asp:Panel ID="contestModeGoalPanel" runat="server" Visible="False">
                    <p>
                        <asp:Label ID="contestEndGoalLabel" runat="server" Text="Statistic Goal:"></asp:Label>
                        <asp:TextBox ID="txbContestEndGoal" runat="server" CssClass="textEntry"></asp:TextBox>
                    </p>
                </asp:Panel>
            <p __designer:mapid="33">
                        <asp:CheckBox ID="chkContestSearchable" runat="server" Checked="True" 
                            Text="Public Contest" />
                    </p>
        </fieldset>
        
            <asp:Button ID="CreateContestButton" runat="server" CommandName="MoveNext" Text="Create Contest" 
                ValidationGroup="CreateContestValidationGroup" OnClick="CreateContest" />     
    </div>

</asp:Content>
