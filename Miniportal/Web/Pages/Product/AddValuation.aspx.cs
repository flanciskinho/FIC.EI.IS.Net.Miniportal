using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.MiniPortal.Model.ValuationService;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Xml.Xsl;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.Product
{
    public partial class AddValuation : System.Web.UI.Page
    {
        private long id;
        private long userId;
        private string sellerId;

        private static IProductService productService;
        private static IValuationService valuationService;
        private static ICommentService commentService;


        static AddValuation()
        {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            valuationService = container.Resolve<IValuationService>();
            productService = container.Resolve<IProductService>();
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
                this.pnlError.Visible = true;
                return;
            }

            if (!Int64.TryParse(Request.Params.Get("id"), out id))
            {
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

            sellerId = aspXml.XPathNavigator.SelectSingleNode("//product/@owner").Value;
            this.aspXml.Visible = true;

            try
            {
                valuationService.getByUserIdAndProductId(userId, id);
                // Decimos que ya tiene valoracion
                this.pnlInfo.Visible = true;
            }
            catch (InstanceNotFoundException)
            { 
                // Mostramos el formulatio
                for (int i = 1; i <= 5; i++ )
                {
                    ddlValuation.Items.Add(new ListItem(getNameValuation(i), i+""));
                }
                this.ddlValuation.Items.FindByValue((int)EValuation.NORMAL + "").Selected = true;
                this.btnAddValuation.Text = GetLocalResourceObject("AddAssessment").ToString();
                this.pnlForm.Visible = true;
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

        protected void BtnAddValuationClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    
                    valuationService.addValuation(userId, sellerId, id, Convert.ToInt64(ddlValuation.SelectedValue), txtComment.Text);
                    this.pnlForm.Visible = false;
                    this.pnlAddRight.Visible = true;
                }
                catch (DuplicateInstanceException)
                {
                    this.pnlForm.Visible = false;
                    this.aspXml.Visible = this.pnlInfo.Visible = true;
                }
                catch
                {
                    Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Errors/InternalError.aspx"));
                }
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
    }
}