<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true" CodeBehind="AddValuation.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.Product.AddValuation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">


    <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
        <div class="alert alert-warning">
            <asp:Label ID="warningProductNotFound" runat="server"
               Text="<%$ Resources:Common, ProductNotFound %>" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false">
        <div class="alert alert-error">
            <asp:Label ID="lblError" runat="server"
               Text="<%$ Resources:Common, UnauthorizedUser %>" />
        </div>
    </asp:Panel>

        <asp:Panel ID="pnlInfo" runat="server" Visible="false">
        <div class="alert alert-info">
            <asp:Label ID="justRated" runat="server"
               Text="<%$ Resources: , JustRated %>" />
        </div>
    </asp:Panel>

    <asp:Xml ID="aspXml" runat="server" Visible="false"></asp:Xml>

    <asp:Panel ID="pnlAddRight" runat="server" Visible="false">
        <div class="alert alert-success">
            <asp:Label ID="AddRated" runat="server"
               Text="<%$ Resources: , AddRated %>" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlForm" runat="server" Visible="false">
        <form ID="addValuationForm" method="POST" runat="server">
            <div class="field">
                <span class="oldLabel">
                    <asp:Localize ID="lblValuation" runat="server" 
                        Text="<%$ Resources: Common, Assessment %>" />
                </span>
                <span class="entry">
                    <asp:DropDownList ID="ddlValuation" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvValuation" runat="server"
                        ControlToValidate="ddlValuation" Display="Dynamic" Text="Campo requerido"/>
                </span>
            </div>
            <div class="field">
                <span class="oldLabel">
                    <asp:Localize ID="lblComment" runat="server" 
                        Text="<%$ Resources: Common, Comment %>" />
                </span>
                <span class="entry">
                    <asp:TextBox ID="txtComment" runat="server" Columns="25" Rows="5"></asp:TextBox>
                </span>
            </div>
            <div class="button">
                <asp:Button ID="btnAddValuation" runat="server" OnClick="BtnAddValuationClick" class="btn btn-large btn-primary" />
            </div>
        </form>
    </asp:Panel>

</asp:Content>
