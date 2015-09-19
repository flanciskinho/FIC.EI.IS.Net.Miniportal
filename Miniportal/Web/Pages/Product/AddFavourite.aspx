<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true"
    CodeBehind="AddFavourite.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.User.AddFavourite"
    meta:resourcekey="Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
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
            <asp:Label ID="lbljustFavourite" runat="server" >
                <asp:Localize ID="lclzJustFavourite" runat="server"
                    Text="<%$ Resources: , JustFavourite %>" />
                &nbsp;  
                <asp:HyperLink ID="favourites" runat="server"
                    Text="<%$ Resources:Common, ManageFavorites%>"
                    NavigateUrl="~/Pages/User/ManageFavourite.aspx"/>
            </asp:Label>
        </div>
    </asp:Panel>

    <asp:Xml ID="aspXml" runat="server" Visible="false"></asp:Xml>

    <asp:Panel ID="pnlAddRight" runat="server" Visible="false">
        <div class="alert alert-success">
            <asp:Label ID="Label1" runat="server"
                Text="<%$ Resources: , AddWishList %>" />
        </div>
    </asp:Panel>


    <asp:Panel ID="pnlForm" runat="server" Visible="false">
        <form ID="addFavouriteForm" method="POST" runat="server">
            <div class="field">
                <span class="oldLabel">
                    <asp:Localize ID="lblNameFavourite" runat="server" 
                    Text="<%$ Resources:Common, Name %>" />
                </span>
                <span class="entry">
                    <asp:TextBox ID="txtNameFavourite" runat="server" Columns="25"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNameFavourite" runat="server"
                        ControlToValidate="txtNameFavourite" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
                </span>
            </div>
            <div class="field">
                <span class="oldLabel">
                    <asp:Localize ID="lblComment" runat="server" 
                    Text="<%$ Resources:Common, Comment %>" />
                </span>
                <span class="entry">
                    <asp:TextBox ID="txtComment" runat="server" Columns="25" Rows="5"></asp:TextBox>
                </span>
            </div>
            <div class="button">
                <asp:Button ID="btnAddFavourite" runat="server" OnClick="BtnAddFavouriteClick" class="btn btn-large btn-primary" />
            </div>
        </form>
    </asp:Panel>

</asp:Content>
