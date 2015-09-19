<%@ Page Language="C#" MasterPageFile="~/Miniportal.Master" AutoEventWireup="true"
    Codebehind="MainPage.aspx.cs" Inherits="Es.Udc.DotNet.MiniPortal.Web.Pages.MainPage" meta:resourcekey="Page" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_BodyContent"
    runat="server">

    <p>
        <asp:Label ID="lblSearchButton" meta:resourcekey="lblSearchButton" runat="server" Visible="false"></asp:Label>
    </p>

    <form id="form1" runat="server">
        <div class="form-horizontal "> <!--  input-append -->
            <asp:TextBox ID="searchBar" meta:resourcekey="searchBar"  runat="server" Columns="25" />
        
            <asp:Button ID="searchButton" runat="server" meta:resourcekey="searchButton"
                OnClick="searchButton_OnClick"
                class="btn btn-default btn-primary" />

            <asp:Button ID="statisticButton" runat="server" meta:resourcekey="showStatistics"
                OnClick="showStatistics_OnClick"
                class="btn btn-default"/>
                

        </div>
    </form>


    <asp:Localize ID="lclContent" runat="server" meta:resourcekey="lclContent" />

</asp:Content>
