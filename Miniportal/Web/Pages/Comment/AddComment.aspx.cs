using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using System.Xml.Xsl;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.Comment
{
    public partial class AddComment : System.Web.UI.Page
    {
        private long userId;
        private long productId;

        private static ICommentService commentService;
        private static IProductService productService;


        static AddComment()
        {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            productService = container.Resolve<IProductService>();
            commentService = container.Resolve<ICommentService>();

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = GetLocalResourceObject("title").ToString();

            //No esta autenticado
            try
            {
                userId = SessionManager.GetUserSession(Context).UserProfileId;
            }
            catch
            {
                this.pnlNonAuthenticated.Visible = true;
                return;
            }
            //No se puede pillar el producto
            if (!Int64.TryParse(Request.Params.Get("id"), out productId))
            {
                //this.aspXml.Visible = this.pnlInfo.Visible = this.pnlError.Visible = this.pnlForm.Visible = this.pnlAddRight.Visible= false;
                this.pnlNotFound.Visible = true;
                return;
            }

            try
            {
                aspXml.Visible = true;
                //Traemos los productos
                aspXml.XPathNavigator = productService.searchProductByIdXML(productId);
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
                this.pnlNotFound.Visible = true;
                return;
            }


            this.pnlForm.Visible = true;
        }

        private XsltArgumentList CreateArgumentListAndParams()
        {
            XsltArgumentList args = new XsltArgumentList();

            foreach (string str in ProductArguments.getParamProductDetails())
                args.AddParam(str, "", GetGlobalResourceObject("Common", str).ToString());


            args.AddParam("ProductId", "", productId);

            args.AddExtensionObject("ext:ProductList", commentService);

            return args;
        }

        protected void BtnCommentClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    List<String> labels;

                    if (!String.IsNullOrWhiteSpace(writeLabel.Text))
                        labels = writeLabel.Text.Split(',').ToList<String>();
                    else
                        labels = null;

                    commentService.addComment(userId, productId, this.writeComment.Text, labels);

                    this.pnlForm.Visible = false;
                    this.pnlAddComment.Visible = true;
                }
                catch (InstanceNotFoundException)
                {
                    this.pnlForm.Visible = true;
                    this.pnlNonAuthenticated.Visible = true;
                }
            }

        }
    }
}