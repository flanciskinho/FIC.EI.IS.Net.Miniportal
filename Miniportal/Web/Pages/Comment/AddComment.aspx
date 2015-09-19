<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true" CodeBehind="AddComment.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.Comment.AddComment" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
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
                Text="<%$ Resources:Common, ProductNotFound %>" />
        </strong></div>
    </asp:Panel>

    <asp:Panel ID="pnlAddComment" runat="server" Visible="false">
        <div class="alert alert-success"><strong>
            <asp:Localize ID="Localize1" runat="server"
                Text="<%$ Resources: , AddedCorrectly %>"/>
        </strong></div>
    </asp:Panel>

    <asp:Xml ID="aspXml" runat="server" Visible="false"></asp:Xml>

    <asp:Panel ID="pnlForm" runat="server" Visible="false">
        <form ID="addCommentForm" method="POST" runat="server">
            <asp:TextBox ID="writeComment" runat="server" Width="600px" Height="80px" TextMode="MultiLine"
                class="text-left" 
                placeholder="<%$ Resources:Common, Comment %>" />


            <asp:RequiredFieldValidator ID="rfvWriteComment" runat="server"
                ControlToValidate="writeComment" Display="Dynamic"
                Text="<%$ Resources:Common, mandatoryField %>"/>
            
            <asp:TextBox ID="writeLabel" runat="server" Width="600px" Height="30px"  TextMode="MultiLine" 
                class="text-left" 
                placeholder="<%$ Resources:Common, TagSeparated %>" />

            <div class="button">
                <asp:Button ID="commentButton" runat="server" OnClick="BtnCommentClick"
                    class="btn btn-large btn-primary"
                    Text="<%$ Resources: , Add %>" /> 
            </div>
        </form>
    </asp:Panel>
            
</asp:Content>
