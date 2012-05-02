<%@ Page Title="About Us" Language="C#" MasterPageFile="~/About.Master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="ActivEarth.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
     <h1><b>About</b></h1>
     <div class="contentBox">
          <div class="contentText">
          <br />
                <p class="centeredImage">
             <asp:Image id="Img2" ImageUrl="~/Images/Home/ActivEarthLogo1.gif" Runat="server" Height= "260px" Width= "340px"/>
              </p>

          <h5><p align=left>    ActivEarth is about promoting global active transportation and recreation as 
              means to better health, improved environments, and sustainable economies. ActivEarth 
              allows you to get connected with your community through being active outside.</p>  

            <p align=left>    Any type of physical activity promotes <b>health</b> and reduces the risk of many 
            chronic diseases as well as obesity.  Being physically active through active 
            transportation not only keeps you in better shape, it can also improve our <b>environment</b> 
            by reducing the use of expensive fossil fuels and the resulting release of green house 
            gases that affect climate change.  <b>Economically</b>, the physically active individual will
            save money by spending less on health care and gasoline.  This translates to lower 
            expenditure at the national level for health care and less reliance on import of 
            fossil fuels.  </p>

            <p align=left><b><em>    Thus, physical activity improves health, economic status, and the environment -- 
            all at the personal and global level.</em></b></p>

            <p align=left>    The ActivEarth application helps individuals keep track of their use of active 
            and public transportation as well as their recreational physical activity.  It will 
            translate this physical activity to health, environmental and financial credits for 
            each person and display graphically in their profile.  Players can earn badges as they 
            achieve various milestones.  In addition, players are encouraged to compete and collaborate 
            with others in their group to demonstrate their collective benefits on health, the 
            environment and the economy.</p></h5><br />
         

        </div>
</asp:Content>
