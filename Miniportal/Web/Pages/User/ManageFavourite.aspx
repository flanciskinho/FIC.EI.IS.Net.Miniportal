<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true"
    CodeBehind="ManageFavourite.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.User.ManageFavourite" 
    meta:resourcekey="Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">

    <asp:Panel ID="pnlInfo" runat="server" Visible="false">
        <div class="alert alert-info">
            <asp:Label ID="noHaveFavourite" runat="server"
                Text="<%$ Resources: , noHaveFavourite.Text %>" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlError" runat="server" Visible="false">
        <div class="alert alert-error">
            <asp:Label ID="lblError" runat="server"
               Text="<%$ Resources:Common, UnauthorizedUser %>" />
        </div>
    </asp:Panel>


    <form ID="frmFavourites" runat="server">
        <asp:GridView ID="gvFavourites" runat="server" CssClass="table"
            GridLines="None"
            DataKeyNames="favouriteId"
            AutoGenerateColumns="false"
            OnRowDeleting="DeleteRecord">
            <Columns>
                <asp:HyperLinkField DataTextField="name"
                    DataNavigateUrlFields="productId"
                    DataNavigateUrlFormatString="~/Pages/Product/ProductDetails.aspx?id={0}"
                    HeaderText="<%$ Resources:Common, Name %>" />

                
                <asp:BoundField DataField="comment"
                    HeaderText="<%$ Resources:Common, Comment %>" />

                <asp:BoundField DataField="addDate"
                    HeaderText="<%$ Resources:Common, Date %>" />

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkButton" runat="server"
                            CommandName="Delete" CssClass="btn btn-danger"
                            OnClientClick=<%# String.Format("return confirm('{0}');", GetLocalResourceObject("Confirm_Delete")) %> >
                            <span class="icon-trash icon-white" > </span>
                            &#160;
                            <asp:Localize ID="lclDelete" runat="server" Text="<%$ Resources:Common, Delete %>" />
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </form>
<%--
    <div class="pagination text-center">
        <ul>
            <li>
                <asp:HyperLink ID="previous" runat="server" Visible="false"
                    Text="<%$ Resources:Common, Previous %>" />
            </li>
            <li>
                <asp:HyperLink ID="next"     runat="server" Visible="false" 
                    Text="<%$ Resources:Common, Next %>" />
            </li>
        </ul>
    </div>
--%>
    <asp:Localize ID="pagination" runat="server"></asp:Localize>

</asp:Content>

