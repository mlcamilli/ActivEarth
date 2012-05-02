<%@ Page Title="About Us" Language="C#" MasterPageFile="~/About.Master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="ActivEarth.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2> About </h2>
     <div class="contentBox">
        	<div class="contentTitle">About</div>
          <div class="contentText">
           <p class="centeredImage">

             <asp:Image id="Img2" ImageUrl="~/Images/Home/ActivEarthLogo1.gif" Runat="server"/>
              </p>
          <h5><p>Active earth is..... </p></h5><br />
        <p>Get involved by....</p><br />
        </div>
</asp:Content>
