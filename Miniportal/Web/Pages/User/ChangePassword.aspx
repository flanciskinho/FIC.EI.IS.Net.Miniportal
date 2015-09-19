<%@ Page Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true"
    Codebehind="ChangePassword.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.User.ChangePassword"
    meta:resourcekey="Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation"
    runat="server">
    &nbsp;-&nbsp;
    <asp:Localize ID="lclMenuExplanation" runat="server" meta:resourcekey="lclMenuExplanation" />
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_BodyContent"
    runat="server">
    <div id="form">

        <form id="ChangePasswordForm" method="post" runat="server" role="form">
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclOldPassword" runat="server" meta:resourcekey="lclOldPassword" />
                </span>
                <span class="entry">
                        <asp:TextBox ID="txtOldPassword" TextMode="Password" runat="server" Columns="16"
                            meta:resourcekey="OldPassword"
                            class="form-control">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOldPassword" runat="server" ControlToValidate="txtOldPassword"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
                        <asp:Label ID="lblOldPasswordError" runat="server" ForeColor="Red" Visible="False"
                            meta:resourcekey="lblOldPasswordError">
                        </asp:Label>
                </span>
            </div>
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclNewPassword" runat="server" meta:resourcekey="lclNewPassword" /></span><span
                        class="entry">
                        <asp:TextBox TextMode="Password" ID="txtNewPassword" runat="server" Columns="16"
                            meta:resourcekey="NewPassword"
                            class="form-control">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
                        <asp:CompareValidator ID="cvCreateNewPassword" runat="server" ControlToCompare="txtOldPassword"
                            ControlToValidate="txtNewPassword" Operator="NotEqual" meta:resourcekey="cvCreateNewPassword"></asp:CompareValidator>
                    </span>
            </div>
            <div class="field form-group">
                <span class="oldLabel">
                    <asp:Localize ID="lclRetypePassword" runat="server" meta:resourcekey="lclRetypePassword" /></span><span
                        class="entry">
                        <asp:TextBox TextMode="Password" ID="txtRetypePassword" runat="server"  Columns="16"
                            meta:resourcekey="RetypePassword"
                            class="form-control">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRetypePassword" runat="server" ControlToValidate="txtRetypePassword"
                            Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
                        <asp:CompareValidator ID="cvPasswordCheck" runat="server" ControlToCompare="txtNewPassword"
                            ControlToValidate="txtRetypePassword" meta:resourcekey="cvPasswordCheck"></asp:CompareValidator>
                    </span>
            </div>
            <div class="button">
                <asp:Button ID="btnChangePassword" runat="server" OnClick="BtnChangePasswordClick"
                    meta:resourcekey="btnChangePassword"
                class="btn btn-large btn-primary" />
            </div>
        </form>
    </div>
</asp:Content>
