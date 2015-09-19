<%@ Page Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true"
    Codebehind="Authentication.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.User.Authentication"
    meta:resourcekey="Page" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation"
    runat="server">
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_BodyContent"
    runat="server">


<div id="form" class="container text-left" > <!-- container-->
      <form id="AuthenticationForm" method="POST" runat="server" class="form-signin">
        <h2 class="form-signin-heading">
            <asp:Localize ID="login" runat="server"
                Text="<%$ Resources: , Login %>" />
        </h2>

        <div class="input-block-level">
         <span class="entry">
            <asp:TextBox ID="txtLogin" runat="server" 
                placeholder="<%$ Resources:Common, Username %>" />
            <asp:RequiredFieldValidator ID="rfvLogin" runat="server"
                ControlToValidate="txtLogin" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
            <asp:Label ID="lblLoginError" runat="server" ForeColor="Red" Style="position: relative"
                Visible="False" meta:resourcekey="lblLoginError">                        
            </asp:Label>
         </span>
        </div>
        <div class="input-block-level">
          <span class="entry">
            <asp:TextBox TextMode="Password" ID="txtPassword" runat="server"
                placeholder="<%$ Resources:Common, Password %>" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                ControlToValidate="txtPassword" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
            <asp:Label ID="lblPasswordError" runat="server" ForeColor="Red" Style="position: relative"
                Visible="False" meta:resourcekey="lblPasswordError">       
            </asp:Label>
         </span>
        </div>


     <label class="checkbox">
        <asp:CheckBox ID="checkRememberPassword" runat="server" TextAlign="Left"/>
        <asp:Localize runat="server" meta:resourcekey="checkRememberPassword" />
        &nbsp;
        <asp:HyperLink ID="lnkRegister" runat="server"
                 NavigateUrl="~/Pages/User/Register.aspx"
                 meta:resourcekey="lnkRegister" />
     </label>   

      <asp:Button ID="btnLogin" runat="server" OnClick="BtnLoginClick" meta:resourcekey="btnLogin"
             class="btn btn-large btn-primary"/>

        <!--
        <input type="text" class="input-block-level" placeholder="Login" />
        <input type="password" class="input-block-level" placeholder="Contrase�a" />
        <label class="checkbox">
          <input type="checkbox" value="remember-me" /> Recordar
          <a class="pull-right" href="#">Crear una cuenta</a>
        </label>
        <button class="btn btn-large btn-primary" type="submit">Iniciar sesi�n</button>
        -->
      </form>
</div>

<%-- 
    <asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Pages/User/Register.aspx" meta:resourcekey="lnkRegister" />
    <div id="form">
        <form id="AuthenticationForm" method="POST" runat="server">
            <div class="field">
                <span class="label">
                    <asp:Localize ID="lclLogin" runat="server" meta:resourcekey="lclLogin" /></span><span
                        class="entry">
                        <asp:TextBox ID="txtLogin" runat="server" Width="100" Columns="16"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLogin" runat="server"
                            ControlToValidate="txtLogin" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
                        <asp:Label ID="lblLoginError" runat="server" ForeColor="Red" Style="position: relative"
                            Visible="False" meta:resourcekey="lblLoginError">                        
                        </asp:Label>
                    </span>
            </div>
            <div class="field">
                <span class="label">
                    <asp:Localize ID="lclPassword" runat="server" meta:resourcekey="lclPassword" /></span><span
                        class="entry">
                        <asp:TextBox TextMode="Password" ID="txtPassword" runat="server" Width="100" Columns="16"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                            ControlToValidate="txtPassword" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
                        <asp:Label ID="lblPasswordError" runat="server" ForeColor="Red" Style="position: relative"
                            Visible="False" meta:resourcekey="lblPasswordError">       
                        </asp:Label>
                    </span>
            </div>
            <div class="checkbox">
                <asp:CheckBox ID="checkRememberPassword" runat="server" TextAlign="Left" meta:resourcekey="checkRememberPassword" />
            </div>
            <div class="button">
                <asp:Button ID="btnLogin" runat="server" OnClick="BtnLoginClick" meta:resourcekey="btnLogin" />
            </div>
        </form>
    </div>

    
--%>
</asp:Content>
