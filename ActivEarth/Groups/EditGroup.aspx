<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.Master" AutoEventWireup="true" CodeBehind="EditGroup.aspx.cs" Inherits="ActivEarth.Groups.EditGroup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2>
                        Manage Group
                    </h2>
                    <p>
                        Use the form below to manage your Group's information.
                    </p>
                    <p>
                        Add hashtags so that users will be able to search those terms to find your Group.
                    </p>

                    <span class="failureNotification">
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </span>
                    <asp:ValidationSummary ID="RegisterGroupValidationSummary" runat="server" CssClass="failureNotification" 
                         ValidationGroup="RegisterGroupValidationGroup"/>
                    <div class="groupInfo">
                        <fieldset class="register">
                            <legend>Group Information</legend>
                            <p>
                                <asp:Label ID="GroupNameLabel" runat="server" AssociatedControlID="txbGroupName">Group Name:</asp:Label>
                                <asp:TextBox ID="txbGroupName" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="GroupNameRequired" runat="server" ControlToValidate="txbGroupName" 
                                     CssClass="failureNotification" ErrorMessage="Group Name is required." ToolTip="Group Name is required." 
                                     ValidationGroup="RegisterGroupValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="DescriptionLabel" runat="server" AssociatedControlID="txbDescription">Description:</asp:Label>
                                <asp:TextBox ID="txbDescription" runat="server" CssClass="textEntry" 
                                    TextMode="MultiLine" Width="90%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="DescriptionRequired" runat="server" ControlToValidate="txbDescription" 
                                     CssClass="failureNotification" ErrorMessage="Description is required." ToolTip="Description is required." 
                                     ValidationGroup="RegisterGroupValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="lblHashTag" runat="server" >New Hashtag:</asp:Label><br />
                                <asp:TextBox ID="txbHashTag" runat="server"></asp:TextBox>
                                <asp:Button ID="AddHashTagButton" CssClass="Button" runat="server" Text="Add Hashtag" OnClick="AddHashTag"/>
                            </p>
                            <p>
                                <asp:Label ID="lblHashTags" runat="server" >Hashtags:</asp:Label><br />
                                <asp:Label ID="lblAllHashTags" runat="server"></asp:Label>
                                <asp:Button ID="ClearHashTagsButton" CssClass="Button" runat="server" Text="Remove Hashtags" OnClick="RemoveHashTags" />  
                            </p>  
                            <p>
                                <asp:Button ID="BootMembersButton" CssClass="Button" runat="server" Text="Manage Group Members" OnClick="BootMembers" />  
                            </p>
                        </fieldset>
                  
                            <asp:Button ID="EditGroupButton" CssClass="Button" runat="server" CommandName="MoveNext" Text="Edit Group Information" 
                                 ValidationGroup="RegisterGroupValidationGroup" OnClick="EditGroupInformation" />
                            <asp:Button ID="DeleteButton" CssClass="Button" runat="server" Text="Delete Group" OnClick="DeleteGroup" />
                            <asp:Button ID="CancelButton" CssClass="Button" runat="server" Text="Cancel" OnClick="Cancel" />
                    </div>
                
          
   
</asp:Content>
