<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true" CodeBehind="AuctionNoRespondingError.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.Errors.AuctionNoRespondingError" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">
    <div class="alert alert-error">
    <asp:Localize ID="errorMsg" runat="server" Text="<%$ Resources:Common, ServerNotResponding %>" />
    </div>
</asp:Content>
