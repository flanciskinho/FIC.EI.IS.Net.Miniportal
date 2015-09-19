<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Miniportal.Master" CodeBehind="ProductDetails.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.Product.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">

    <asp:Label ID="ProductNotFound" runat="server" Visible="false"
            text="<%$ Resources:Common, ProductNotFound %>"
            class="alert alert-warning" />
    <asp:Xml ID="aspXml" runat="server"></asp:Xml>

     <div class="pager">
            <ul>
                <li>
                   <asp:HyperLink ID="valuation" runat="server" Visible="false"
                       Text="<%$ Resources: , AddAssessment %>" />
                </li>
                <li>
                   <asp:HyperLink ID="addFavourite" runat="server" Visible="false" 
                       Text="<%$ Resources: , AddWishList %>" />
                </li>
                <li>
                   <asp:HyperLink ID="comment" runat="server" Visible="false" 
                       Text="<%$ Resources: , SeeComment %>" />
                </li>
                <li>
                    <asp:HyperLink ID="doBid" runat="server" Visible="false"
                       Text="<%$ Resources: , DoBid %>" />
                </li>
                <li>
                    <asp:HyperLink ID="addComment" runat="server" Visible="false"
                       Text="<%$ Resources: , addCommentButton %>" />
                </li>
            </ul>
        </div>
</asp:Content>
