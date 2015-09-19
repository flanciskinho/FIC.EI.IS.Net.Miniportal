<%@ Page Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true"
    Codebehind="Register.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.User.Register" 
    meta:resourcekey="Page" culture="auto" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation"
    runat="server">
    &nbsp;-&nbsp;
    <asp:Localize ID="lclMenuExplanation" runat="server" meta:resourcekey="lclMenuExplanation" />
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_BodyContent"
    runat="server">
    <div id="form">
        <form id="RegisterForm" method="post" runat="server" role="form">
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclUserName" runat="server" meta:resourcekey="lclUserName" /></span><span
                        class="entry">
                        <asp:TextBox ID="txtLogin" runat="server" Width="100px" Columns="16" 
                            placeholder="<%$ Resources: , lclUserName.Text %>"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtLogin"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>" 
                    meta:resourcekey="rfvUserNameResource1"></asp:RequiredFieldValidator>
                        <asp:Label ID="lblLoginError" runat="server" ForeColor="Red" Style="position: relative"
                            Visible="False" meta:resourcekey="lblLoginError"></asp:Label></span>
            </div>
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclPassword" runat="server" meta:resourcekey="lclPassword" /></span><span
                        class="entry">
                        <asp:TextBox TextMode="Password" ID="txtPassword" runat="server" 
                    Width="100px" Columns="16" placeholder="<%$ Resources: , lclPassword.Text %>"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>" 
                    meta:resourcekey="rfvPasswordResource1"></asp:RequiredFieldValidator></span>
            </div>
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclRetypePassword" runat="server" meta:resourcekey="lclRetypePassword" /></span><span
                        class="entry">
                        <asp:TextBox TextMode="Password" ID="txtRetypePassword" runat="server" Width="100px"
                            Columns="16" placeholder="<%$ Resources: , lclRetypePassword.Text %>"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRetypePassword" runat="server" ControlToValidate="txtRetypePassword"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>" 
                    meta:resourcekey="rfvRetypePasswordResource1"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvPasswordCheck" runat="server" ControlToCompare="txtPassword"
                            ControlToValidate="txtRetypePassword" meta:resourcekey="cvPasswordCheck"></asp:CompareValidator></span>
            </div>
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclFirstName" runat="server" meta:resourcekey="lclFirstName" /></span><span
                        class="entry">
                        <asp:TextBox ID="txtFirstName" runat="server" Width="100px" 
                    Columns="16" placeholder="<%$ Resources: , lclFirstName.Text %>" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>" 
                    meta:resourcekey="rfvFirstNameResource1"></asp:RequiredFieldValidator></span>
            </div>
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclSurname" runat="server" meta:resourcekey="lclSurname" /></span><span
                        class="entry">
                        <asp:TextBox ID="txtSurname" runat="server" Width="100px" Columns="16" 
                            placeholder="<%$ Resources: , lclSurname.Text %>" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSurname" runat="server" ControlToValidate="txtSurname"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>" 
                    meta:resourcekey="rfvSurnameResource1"></asp:RequiredFieldValidator></span>
            </div>
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclEmail" runat="server" meta:resourcekey="lclEmail" /></span><span
                        class="entry">
                        <asp:TextBox ID="txtEmail" runat="server" Width="100px" Columns="16" 
                            placeholder="<%$ Resources: , lclEmail.Text %>" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>" 
                    meta:resourcekey="rfvEmailResource1"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                            Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            meta:resourcekey="revEmail"></asp:RegularExpressionValidator></span>
            </div>
            <div class="button">
                <asp:Button ID="btnRegister" runat="server" OnClick="BtnRegisterClick" meta:resourcekey="btnRegister" class="btn btn-large btn-primary"/>
            </div>
        </form>
    </div>
</asp:Content>
