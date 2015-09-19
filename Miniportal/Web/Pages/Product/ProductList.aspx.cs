using System;
using System.Collections.Generic;

using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.MiniPortal.Web.Properties;
using Microsoft.Practices.Unity;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.MiniPortal.Model.ValuationService;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.Product
{
    public partial class ProductList : System.Web.UI.Page
    {
        private List<Model.ProductService.Product> list;
        private static ICommentService commentService;
        private static IProductService productService;

        static ProductList()
        {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            productService   = container.Resolve<IProductService>();
            commentService   = container.Resolve<ICommentService>();

        }

        //private void showNextPrevious(string keywords, int start, int size, int total, bool stat)
        //{
        //    if ((start - size) >= 0)
        //    {
        //        string url = HttpContext.Current.Request.Url.AbsolutePath +
        //            "?keyword=" + keywords +
        //            "&start=" + (start - size);
        //        //if (size != Settings.Default.DefaultPageSize)
        //            url += "&size=" + size;
        //        if (stat)
        //            url += "&stat=1";

        //        this.previous.NavigateUrl = Response.ApplyAppPathModifier(url);
        //        this.previous.Visible = true;
        //    }

        //    if (total > (start + size))
        //    {
        //        string url = HttpContext.Current.Request.Url.AbsolutePath +
        //            "?keyword=" + keywords +
        //            "&start=" + (start + size);
        //        //if (size != Settings.Default.DefaultPageSize)
        //            url += "&size=" + size;
        //        if (stat)
        //            url += "&stat=1";

        //        this.next.NavigateUrl = Response.ApplyAppPathModifier(url);
        //        this.next.Visible = true;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                Page.Title = Page.Title + " - " + GetLocalResourceObject("title").ToString();

                string keywords = Request.QueryString["keyword"];
                int start, size;
                if (!Int32.TryParse(Request.Params.Get("start"), out start))
                    start = 0;
                if (!Int32.TryParse(Request.Params.Get("size"), out size))
                    size = Settings.Default.DefaultPageSize;

                if (size <= 0)
                    size = Settings.Default.DefaultPageSize;
                if (start < 0)
                    start = 0;

                bool stat = "1".Equals(Request.QueryString["stat"]);

                int total = (stat) ?
                    pageLoadStat(keywords, start, size):
                    pageLoadXSLT(keywords, start, size);

                if (total == -1)
                    return;

                if (start >= total)
                    start = total - size;

                //showNextPrevious(keywords, start, size, total, stat);

                string url = HttpContext.Current.Request.Url.AbsolutePath +
                        "?keyword=" + keywords +
                        "&start={0}" +
                        "&size=" + size;
                if (stat)
                    url += "&stat=1";

                pagination.Text = Pagination.showNextPrevious(start, size, total, url, Response);
                //showNextPrevious(int start, int size, int total, string url, HttpResponse Response)
            }
            catch (ServerNotRespondingException)
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Errors/AuctionNoRespondingError.aspx"));
            }
        }



        /// <exception cref="ServerNotRespondingException" />
        private int pageLoadStat(string keywords, int start, int size)
        {
                int total = productService.getNumberOfProducts(keywords);

                if (total == 0)
                {
                    this.lblProductError.Text = GetLocalResourceObject("NoProducts").ToString();
                    this.lblProductError.Visible = true;
                    return -1;
                }

                if (start >= total)
                    start = total - size;

                this.searchWithStat.Visible = true;

                list = productService.searchProductByKeywords(keywords, start, size);

                this.ProductListRepeater.DataSource = list;
                this.ProductListRepeater.DataBind();

                return total;
        }

        protected void onItemDataBoundProduct(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal valuation = e.Item.FindControl("valuation") as Literal;
                if (list[e.Item.ItemIndex].numberOfValuation == 0)
                {
                    valuation.Text = "(" + GetGlobalResourceObject("Common", "NoAssessment").ToString() + ")";
                }
                else
                {
                    valuation.Text = getNameValuation((int)list[e.Item.ItemIndex].average) + "/";
                    if (list[e.Item.ItemIndex].numberOfValuation == 1)
                    {
                        valuation.Text += "1 "+ GetGlobalResourceObject("Common", "OneAssessment");
                    }
                    else
                    {
                        valuation.Text += list[e.Item.ItemIndex].numberOfValuation + " " + GetGlobalResourceObject("Common", "MoreAssessment");
                    }
                }

                HyperLink seller = e.Item.FindControl("linkSeller") as HyperLink;
                seller.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Valuation/SeeValuation.aspx?seller={0}", list[e.Item.ItemIndex].sellerId));
                
                HyperLink product = e.Item.FindControl("linkProduct") as HyperLink;
                product.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Product/ProductDetails.aspx?id={0}", list[e.Item.ItemIndex].productId));

                HyperLink favorite = e.Item.FindControl("linkFavourite") as HyperLink;
                favorite.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Product/AddFavourite.aspx?id={0}", list[e.Item.ItemIndex].productId));

                HyperLink comment = e.Item.FindControl("linkComment") as HyperLink;
                comment.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Comment/AddComment.aspx?id={0}", list[e.Item.ItemIndex].productId));

                HyperLink assessment = e.Item.FindControl("linkValuation") as HyperLink;
                assessment.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Product/AddValuation.aspx?id={0}", list[e.Item.ItemIndex].productId));

                HyperLink hasComment = e.Item.FindControl("linkHasComment") as HyperLink;
                if (commentService.getNumberOfComment(list[e.Item.ItemIndex].productId) == 0)
                    hasComment.Visible = false;
                else
                    hasComment.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Comment/SeeComment.aspx?id={0}", list[e.Item.ItemIndex].productId));

            }
        }

        private string getNameValuation(int i)
        {
            switch (i)
            {
                case (int)EValuation.HORRIBLE:
                    return GetGlobalResourceObject("Common", "EValuation_Horrible").ToString();
                case (int)EValuation.BAD:
                    return GetGlobalResourceObject("Common", "EValuation_Bad").ToString();
                case (int)EValuation.NORMAL:
                    return GetGlobalResourceObject("Common", "EValuation_Normal").ToString();
                case (int)EValuation.WELL:
                    return GetGlobalResourceObject("Common", "EValuation_Well").ToString();
                case (int)EValuation.EXCELLENT:
                    return GetGlobalResourceObject("Common", "EValuation_Excellent").ToString();
                default:
                    return GetGlobalResourceObject("Common", "EValuation_Horrible").ToString(); ; //Kernel panic
            }
        }

        /// <exception cref="ServerNotRespondingException" />
        private int pageLoadXSLT(string keywords, int start, int size)
        {
            aspXml.Visible = true;

            //Traemos los productos
            aspXml.XPathNavigator = productService.searchProductByKeywordsXML(keywords, start, size);

            // Anadimos los argumentos
            aspXml.TransformArgumentList = CreateArgumentListAndParams();

            // Indicamos la transformacion xslt
            aspXml.TransformSource = Server.MapPath("~/XSLT/ProductList.xslt");

            int total;
            if (!Int32.TryParse(aspXml.XPathNavigator.SelectSingleNode("/products/@total").Value, out total))
            {
                this.lblProductError.Text = GetLocalResourceObject("NoProducts").ToString();
                this.lblProductError.Visible = true;
                return -1;
            }
            if (total == 0)
            {
                this.lblProductError.Text = GetLocalResourceObject("NoProducts").ToString();
                this.lblProductError.Visible = true;
                this.aspXml.Visible = false;
                return -1;
            }

            return total;
        }


        private XsltArgumentList CreateArgumentListAndParams()
        {
            XsltArgumentList args = new XsltArgumentList();

            foreach (string str in ProductArguments.getParamProductList())
                args.AddParam(str, "", GetGlobalResourceObject("Common", str).ToString());

            args.AddExtensionObject("ext:ProductList", commentService);

            return args;
        }

    }
}