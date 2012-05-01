<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.master" AutoEventWireup="True" CodeBehind="Groups.aspx.cs" Inherits="ActivEarth.Groups.Groups" %>

<%@ Register src="GroupsDisplayTable.ascx" tagname="GroupsDisplayTable" tagprefix="uc" %>
<%@ Register src="OwnedGroupsDisplayTable.ascx" tagname="OwnedGroupsDisplayTable" tagprefix="uc2" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<h2>
    Search For Groups:
</h2>
    <div>
    <asp:TextBox ID="searchBox" Placeholder="Enter a Group Name or Hashtag here..." runat="server" CssClass="textEntry" width="100%" OnTextChanged="SearchGroups"></asp:TextBox>
    </div>
    <div style="clear:both;padding-bottom:20px">
    <asp:Button ID="SearchButton" Text="Search" runat="server"  OnClick="SearchGroups" />
    </div>
    
    <div style="clear:both">
<h2>
    Your Groups:
</h2>
    <br/><br/>
    <div>
    <uc:GroupsDisplayTable ID="GroupsDisplayTable1" runat="server" />
    </div>
    </div>
    
    <br /><br />

     <div style="clear:both;padding-top:20px">
     <h2>Group Owner Options:</h2>
     <br /><br />
     <uc2:OwnedGroupsDisplayTable ID="OwnedGroupsDisplayTable1" runat="server"/>
     <br />
     <div style="clear:both">
         <asp:Button ID="CreateGroupButton" CssClass="Button" runat="server" Text="Create a New Group" OnClick="CreateGroup" />                          
     </div>
     </div>
    <noframes>
    <p>This section (everything between the 'noframes' tags) will only be displayed if the users' browser doesn't support frames. You can provide a link to a non-frames version of the website here. Feel free to use HTML tags within this section.</p>
    </noframes>




</asp:Content>