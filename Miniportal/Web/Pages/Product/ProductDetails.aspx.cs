using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using System.Xml.Xsl;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.Product
{
    public partial class ProductDetails : System.Web.UI.Page
    {
        private long userId;
        private int id;

        private static IProductService productService;
        private static ICommentService commentService;


        static ProductDetails() {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            productService   = container.Resolve<IProductService>();
            commentService = container.Resolve<ICommentService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = Page.Title + " - " + GetLocalResourceObject("title").ToString();

            if (!Int32.TryParse(Request.Params.Get("id"), out id))
            {
                this.ProductNotFound.Visible = true;
                return;
            }

            try{
                //Traemos los productos
                aspXml.XPathNavigator = productService.searchProductByIdXML(id);
            }
            catch (ServerNotRespondingException)
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Errors/AuctionNoRespondingError.aspx"));
            }

            // Anadimos los argumentos
            aspXml.TransformArgumentList = CreateArgumentListAndParams();

            // Indicamos la transformacion xslt
            aspXml.TransformSource = Server.MapPath("~/XSLT/ProductDetails.xslt");

            int tmp = aspXml.XPathNavigator.Select("//product").Count;
            if (tmp != 1)
            {
                aspXml.Visible = false;
                this.ProductNotFound.Visible = true;
                this.ProductNotFound.Text = GetGlobalResourceObject("Common", "ProductNotFound").ToString();
                return;
            }
            long productId = Convert.ToInt64(id);
            long numberOfComment = commentService.getNumberOfComment(productId);

            // Mostramos el enlace para pujar
            this.doBid.NavigateUrl = Response.ApplyAppPathModifier(productService.getUrl2Bid(id));
            this.doBid.Visible = true;

            // Mostramos el enlace para valorar
            this.valuation.NavigateUrl = String.Format("~/Pages/Product/AddValuation.aspx?id={0}", id);
            this.valuation.Visible = true;

            // Mostramos el enlace para anadir a favoritos
            this.addFavourite.NavigateUrl = String.Format("~/Pages/Product/AddFavourite.aspx?id={0}", id);
            this.addFavourite.Visible = true;

            // Mostramos el enlace para ver los comentarios
            if (numberOfComment > 0)
            {
                this.comment.NavigateUrl = String.Format("~/Pages/Comment/SeeComment.aspx?id={0}", id);
                this.comment.Visible = true;
            }

            //Mostramos el boton de añadir comentario
            this.addComment.NavigateUrl = String.Format("~/Pages/Comment/AddComment.aspx?id={0}", id);
            this.addComment.Visible = true;

        }

        private XsltArgumentList CreateArgumentListAndParams()
        {
            XsltArgumentList args = new XsltArgumentList();

            foreach (string str in ProductArguments.getParamProductDetails())
                args.AddParam(str, "", GetGlobalResourceObject("Common", str).ToString());

            args.AddParam("ProductId", "", id);

            args.AddExtensionObject("ext:ProductList", commentService);

            return args;
        }
    }
}