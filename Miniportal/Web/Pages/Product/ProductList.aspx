<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Miniportal.Master" CodeBehind="ProductList.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.Product.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">

        <asp:Label ID="lblProductError" runat="server" Visible="false"
            class="alert alert-info" />

        <asp:xml ID="aspXml" runat="server" Visible="false" />
    
        <asp:Panel ID="searchWithStat" runat="server" Visible="false">
         <form ID="frmProductList"  runat="server">
            <table class="table table-bordered table-stripped">
                <thead>
                 <tr>
                   <th>
                     <asp:Localize ID="lclHeadName" runat="server" Text="<%$ Resources:Common, Name %>" />
                   </th>
                   <th>
                     <asp:Localize ID="lclHeadSeller" runat="server" Text="<%$ Resources:Common, Seller %>" />
                   </th>
                   <th>
                     <asp:Localize ID="lclHeadCategory" runat="server" Text="<%$ Resources:Common, Category %>" />
                   </th>
                   <th>
                     <asp:Localize ID="lclHeadPrice" runat="server" Text="<%$ Resources:Common, Price %>" />
                   </th>
                   <th>
                     <asp:Localize ID="lclHeadMinutes" runat="server" Text="<%$ Resources:Common, Minutes2End %>" />
                   </th>
                 </tr>
               </thead>
               <tbody>

                        <asp:Repeater ID="ProductListRepeater" OnItemDataBound="onItemDataBoundProduct" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:HyperLink ID="linkFavourite" runat="server">
                                            <span class="icon-star" ></span>
                                        </asp:HyperLink>
                                        &#160;
                                        <asp:HyperLink ID="linkComment" runat="server">
                                            <span class="icon-comment" ></span>
                                        </asp:HyperLink>
                                        &#160;
                                        <asp:HyperLink ID="linkProduct" runat="server"
                                            Text='<%# Eval("productName") %>' />
                                        &#160;
                                        <asp:HyperLink ID="linkHasComment" runat="server">
                                            <span class="icon-list-alt" ></span>
                                        </asp:HyperLink>
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="linkValuation" runat="server">
                                            <span class="icon-check" ></span>
                                        </asp:HyperLink>
                                        &#160;
                                        <asp:HyperLink ID="linkSeller" runat="server"
                                            Text='<%# Eval("sellerId") %>' />
                                        <br />
                                        <asp:Literal ID="valuation" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="category" runat="server" Text='<%# Eval("category") %>' ></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="price" runat="server" Text='<%# Eval("currentPrice") %>' ></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="minutes" runat="server" Text='<%# Eval("minutesToEnd") %>' ></asp:Literal>
                                    </td>
                                </tr>

                            </ItemTemplate>
                        </asp:Repeater>
                    
               </tbody>
            </table>
            </form>
        </asp:Panel>

    <asp:Localize ID="pagination" runat="server"></asp:Localize>

</asp:Content>
