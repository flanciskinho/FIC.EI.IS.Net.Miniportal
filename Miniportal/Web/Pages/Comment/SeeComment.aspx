<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true" CodeBehind="SeeComment.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.Comment.SeeComment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">


     <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
        <div class="alert alert-error"><strong>
            <asp:Localize ID="lclNotFound" runat="server"
                Text="<%$ Resources:Common, CommentNotFound %>" />
        </strong></div>
    </asp:Panel>

    <asp:Xml ID="aspXml" runat="server" Visible="false"></asp:Xml>

    
    <asp:Localize ID="paginationTop" runat="server"></asp:Localize>

    <form ID="frmSeeComment"  runat="server">
        <asp:Repeater ID="SeeCommentListRepeater" OnItemDataBound="onItemDataBoundSeeComment" runat="server">
            <ItemTemplate>
                <div class="well">
                    <div class="text-left">
                        <strong>
                            <asp:Literal ID="userName" runat="server" Text='<%# Eval("UserProfile.loginName") %>'></asp:Literal>
                        </strong>
                        <small>
                            <asp:Literal ID="addDate"  runat="server" Text='<%# Eval("addDate") %>'></asp:Literal>
                        </small>
                    </div>
                    
                    <div>
                        <asp:Literal ID="comment" runat="server" Text='<%# Eval("txt") %>'></asp:Literal>
                    </div>    

                    <div>
                        <asp:Literal ID="tags" runat="server" Visible="true" />
                    </div> 

                    <asp:Panel ID="pnlDeleteUpdate" runat="server" Visible="false">
                        <asp:HyperLink ID="deleteComment" runat="server"
                            class="alert-danger"
                            Text="<%$ Resources: , Delete %>"/>
                        &#160;
                        <asp:HyperLink ID="updateComment" runat="server" 
                            class="alert-info"
                            Text="<%$ Resources: , Update %>"/>
                    </asp:Panel>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </form>
    
    <asp:Localize ID="pagination" runat="server"></asp:Localize>

</asp:Content>
