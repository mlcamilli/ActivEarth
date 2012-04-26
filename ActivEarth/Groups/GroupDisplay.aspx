<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.master" AutoEventWireup="True" CodeBehind="GroupDisplay.aspx.cs" Inherits="ActivEarth.Groups.GroupDisplay" %>

<%@ Register src="MembersDisplayTable.ascx" tagname="MembersDisplayTable" tagprefix="uc" %>
<%@ Register src="WallDisplay.ascx" tagname="WallDisplay" tagprefix="uc1" %>
<%@ Register src="~/Competition/Contests/ContestDisplayTable.ascx" tagname="ContestDisplayTable" tagprefix="uc2" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
<h2>
    <asp:Label runat="server" ID="lblGroupName"></asp:Label>
</h2>
<h3>
    <asp:Label runat="server" ID="lblDescription"></asp:Label>
</h3>

<div class="groupTitle">

<uc:MembersDisplayTable ID="MembersDisplayTable1" runat="server" />

<uc1:WallDisplay ID="WallDisplay1" runat="server" />

<uc2:ContestDisplayTable ID="ContestDisplayTable1" runat="server" />

</div> 



    <noframes>
    <p>This section (everything between the 'noframes' tags) will only be displayed if the users' browser doesn't support frames. You can provide a link to a non-frames version of the website here. Feel free to use HTML tags within this section.</p>
    </noframes>




</asp:Content>