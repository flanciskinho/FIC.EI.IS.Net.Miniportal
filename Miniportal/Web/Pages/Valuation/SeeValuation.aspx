<%@ Page Title="" Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true" CodeBehind="SeeValuation.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.Valuation.SeeValuation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">
    <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
        <div class="alert alert-error">
            <asp:Label ID="warningProductNotFound" runat="server" 
                 Text="<%$ Resources:, SellerNotFound %>"/>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlValuation" runat="server" Visible="false">
        <table class="table table-bordered">
            <tr>
                <th>
                    <asp:Localize ID="seller" runat="server"
                        Text="<%$ Resources:Common, Seller %>"/>
                </th>
                <td>
                    <asp:Label ID="lblSeller" runat="server" Visible="true" />
                </td>
            </tr>
            <tr>
                <th>
                    <asp:Localize ID="Assessment" runat="server"
                        Text="<%$ Resources:Common, Assessment %>"/>
                </th>
                <td><asp:Label ID="lblValuation" runat="server" /></td>
            </tr>
        </table>
    
    </asp:Panel>


     <form ID="frmValuation"  runat="server">
        <asp:Repeater ID="ValuationListRepeater" OnItemDataBound="onItemDataBoundValuation" runat="server">
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

                    <div class="text-right">
                        <asp:Literal ID="score" runat="server" ></asp:Literal>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </form>

    <asp:Localize ID="pagination" runat="server"></asp:Localize>

</asp:Content>
