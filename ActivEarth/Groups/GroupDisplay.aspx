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
    <asp:Label runat="server" ID="lblDescription"></asp:Label><br /><br />
    <b>Hashtags: </b><asp:Label runat="server" ID="lblHashTags"></asp:Label>
</h3>


<br />
<h2>Members</h2>
<div class="groupTitle" style="padding-bottom:5px">
<uc:MembersDisplayTable ID="MembersDisplayTable1" runat="server" />
</div>
<div style="clear:both;padding-bottom:20px">
<asp:HyperLink ID="hypSeeMore" runat="server" Text="See More"></asp:HyperLink>
</div>


<div>
<h2>Contests</h2>
<uc2:ContestDisplayTable ID="ContestDisplayTable1" runat="server" />
<asp:Label runat="server" ID="EmptyContest" Text="<br/>No active Contests."></asp:Label>
</div> 

<br /><br /><br />

<h2>Recent Activity</h2>
<div>
<asp:TextBox ID="txbTitle" runat="server" Placeholder="Message Title"></asp:TextBox>
<br/>
<asp:TextBox ID="txbMessage" runat="server" TextMode="MultiLine" Width="90%" Placeholder="Enter a message here..."></asp:TextBox>
<br/>
<asp:Button ID="PostButton" runat="server" OnClick="PostMessage" Text="Post"/>
</div>
<div style="clear:both">
<uc1:WallDisplay ID="WallDisplay1" runat="server" />
</div>



<br /><br />



    <noframes>
    <p>This section (everything between the 'noframes' tags) will only be displayed if the users' browser doesn't support frames. You can provide a link to a non-frames version of the website here. Feel free to use HTML tags within this section.</p>
    </noframes>




</asp:Content>