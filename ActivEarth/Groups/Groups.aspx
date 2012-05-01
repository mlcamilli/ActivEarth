<%@ Page Title="" Language="C#" MasterPageFile="~/Tabs.master" AutoEventWireup="True" CodeBehind="Groups.aspx.cs" Inherits="ActivEarth.Groups.Groups" %>

<%@ Register src="GroupsDisplayTable.ascx" tagname="GroupsDisplayTable" tagprefix="uc" %>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
<h2>
    List of Groups for <asp:Label runat="server" ID="lblUserName"></asp:Label>
</h2>
    <br/><br/>
    <div>
    <uc:GroupsDisplayTable ID="GroupsDisplayTable1" runat="server" />
    </div>
    
    <br /><br />
    
    <div>
    <asp:TextBox ID="searchBox" Text="search" runat="server" CssClass="textEntry"></asp:TextBox>
    </div>
    <div>
    <asp:Button ID="SearchButton" Text="Search" runat="server"  OnClick="SearchGroups" />
    </div>

    <noframes>
    <p>This section (everything between the 'noframes' tags) will only be displayed if the users' browser doesn't support frames. You can provide a link to a non-frames version of the website here. Feel free to use HTML tags within this section.</p>
    </noframes>




</asp:Content>