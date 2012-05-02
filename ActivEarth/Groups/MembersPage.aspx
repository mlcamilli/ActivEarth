<%@ Page Title="" Language="C#" MasterPageFile="~/GroupTabs.master" AutoEventWireup="True" CodeBehind="MembersPage.aspx.cs" Inherits="ActivEarth.Groups.MembersPage" %>

<%@ Register src="MembersDisplayTable.ascx" tagname="MembersDisplayTable" tagprefix="uc" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
<h2>
    <asp:Label runat="server" ID="lblGroupName"></asp:Label>
</h2>
<h3>
    All Users:
</h3>
<br />
<div class="groupTitle">

<uc:MembersDisplayTable ID="MembersDisplayTable1" runat="server" />


</div> 



    <noframes>
    <p>This section (everything between the 'noframes' tags) will only be displayed if the users' browser doesn't support frames. You can provide a link to a non-frames version of the website here. Feel free to use HTML tags within this section.</p>
    </noframes>




</asp:Content>