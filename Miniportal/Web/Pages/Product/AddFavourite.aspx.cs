using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Es.Udc.DotNet.MiniPortal.Model.FavouriteService;
using System.Xml.XPath;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Xml.Xsl;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.User
{
    public partial class AddFavourite : System.Web.UI.Page
    {
        private long id;
        private long userId;

        private static IProductService productService;
        private static IFavouriteService favouriteService;
        private static ICommentService commentService;


        static AddFavourite() {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            favouriteService = container.Resolve<IFavouriteService>(); 
            productService   = container.Resolve<IProductService>();
            commentService = container.Resolve<ICommentService>();

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = GetLocalResourceObject("title").ToString();

            try
            {
                userId = SessionManager.GetUserSession(Context).UserProfileId;
            }
            catch
            {
                //this.aspXml.Visible = this.pnlInfo.Visible = this.pnlNotFound.Visible = this.pnlForm.Visible = this.pnlAddRight.Visible = false;
                this.pnlError.Visible = true;
                return;
            }

            if (!Int64.TryParse(Request.Params.Get("id"), out id))
            {
                //this.aspXml.Visible = this.pnlInfo.Visible = this.pnlError.Visible = this.pnlForm.Visible = this.pnlAddRight.Visible= false;
                this.pnlNotFound.Visible = true;
                return;
            }

            try
            {
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
                this.warningProductNotFound.Visible = true;
                this.warningProductNotFound.Text = GetGlobalResourceObject("Common", "ProductNotFound").ToString();
                return;
            }

            this.aspXml.Visible = true;

            try
            {
                favouriteService.getByUserIdAndProductId(userId, id);
                // Decimos que ya esta en favoritos
                this.pnlInfo.Visible = true;
            }
            catch (InstanceNotFoundException)
            {
                // Mostramos el formulario
                this.pnlForm.Visible = true;
                this.txtNameFavourite.Text = aspXml.XPathNavigator.SelectSingleNode("//product/@name").Value;

                this.btnAddFavourite.Text = GetLocalResourceObject("AddFavourite").ToString();
            }

            
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

        protected void BtnAddFavouriteClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    favouriteService.addFavourite(userId, id, this.txtNameFavourite.Text, this.txtComment.Text);
                    this.pnlForm.Visible = false;
                    this.pnlAddRight.Visible = true;
                }
                catch (DuplicateInstanceException)
                {
                    //this.aspXml.Visible = this.pnlInfo.Visible = this.pnlNotFound.Visible = this.pnlForm.Visible = false; this.pnlError.Visible = this.pnlAddRight.Visible = false;
                    this.pnlForm.Visible = false;
                    this.aspXml.Visible = this.pnlInfo.Visible = true;
                }
                catch {

                    Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Errors/InternalError.aspx"));
                }

            }

        }
    }
}