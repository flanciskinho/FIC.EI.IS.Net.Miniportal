<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true" CodeBehind="DeleteComment.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.Comment.DeleteComment" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">

    <asp:Panel ID="pnlNonAuthenticated" runat="server" Visible="false">
        <div class="alert alert-error"><strong>
            <asp:Localize ID="lclNonAutenticated" runat="server"
                Text="<%$ Resources:Common, UnauthorizedUser %>" />
        </strong></div>
    </asp:Panel>

    <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
        <div class="alert alert-error"><strong>
            <asp:Localize ID="lclNotFound" runat="server"
                Text="<%$ Resources:Common, CommentNotFound %>" />
        </strong></div>
    </asp:Panel>

    <asp:Panel ID="pnlDenyPermission" runat="server" Visible="false">
        <div class="alert alert-error"><strong>
            <asp:Localize ID="Localize1" runat="server"
                Text="<%$ Resources:Common, CommentDenyPermission %>" />
        </strong></div>
    </asp:Panel>

    <asp:Panel ID="pnlDeleteSuccess" runat="server" Visible="false">
        <div class="alert alert-success"><strong>
            <asp:Localize ID="lclDeleteSuccess" runat="server"
                Text="<%$ Resources:, DeletedCorrectly %>"/>
        </strong></div>
    </asp:Panel>

    <asp:Xml ID="aspXml" runat="server" Visible="false"></asp:Xml>

    <asp:Panel ID="pnlComment" runat="server" Visible="false">
        <div class="well">
            <div class="text-left">
                <strong>
                    <asp:Literal ID="userName" runat="server" />
                </strong>
                <small>
                    <asp:Literal ID="addDate"  runat="server" />
                </small>
            </div>
                    
            <div>
                <asp:Literal ID="comment" runat="server" />
            </div>
            
            <div>
                <asp:Literal ID="tags" runat="server" Visible="true" />
            </div> 
        </div>
        <form ID="deleteCommentForm" method="POST" runat="server">
            <div class="button">
                <asp:Button ID="commentButton" runat="server" OnClick="BtnDeleteClick"
                    class="btn btn-large btn-danger"
                    Text="<%$ Resources:, Delete %>" /> 
            </div>
        </form>
        
    </asp:Panel>

</asp:Content>
