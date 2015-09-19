using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.MiniPortal.Web.Properties;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using System.Xml.Xsl;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.Comment
{
    public partial class SeeComment : System.Web.UI.Page
    {
        private long productId;
        private long userId;
        private bool authenticated = false;

        private int start, size, total;
        private List<Model.Comment> list;

        private static ICommentService commentService;
        private static IProductService productService;


        static SeeComment()
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
                authenticated = true;
            }
            catch
            {
                authenticated = false;
            }
            //No se puede pillar el producto
            if (!Int64.TryParse(Request.Params.Get("id"), out productId))
            {
                //this.aspXml.Visible = this.pnlInfo.Visible = this.pnlError.Visible = this.pnlForm.Visible = this.pnlAddRight.Visible= false;
                this.pnlNotFound.Visible = true;
                return;
            }
            // para la paginacion
            if (!Int32.TryParse(Request.Params.Get("start"), out start))
                start = 0;
            if (!Int32.TryParse(Request.Params.Get("size"), out size))
                size = Settings.Default.DefaultPageSize;

            try
            {
                aspXml.Visible = true;
                //Traemos los productos
                aspXml.XPathNavigator = productService.searchProductByIdXML(productId);

                // Anadimos los argumentos
                aspXml.TransformArgumentList = CreateArgumentListAndParams();

                // Indicamos la transformacion xslt
                aspXml.TransformSource = Server.MapPath("~/XSLT/ProductDetails.xslt");

                int tmp = aspXml.XPathNavigator.Select("//product").Count;
                if (tmp != 1)
                {
                    aspXml.Visible = false;
                }
            }
            catch (ServerNotRespondingException)
            {
                aspXml.Visible = false;
            }

            // comprobar si hay productos
            total = (int) commentService.getNumberOfComment(productId);
            if (total == 0)
            {
                this.frmSeeComment.Visible = false;
                this.pnlNotFound.Visible = true;
                return;
            }

            // mayor control sobre la paginacion
            if (size <= 0)
                size = Settings.Default.DefaultPageSize;
            if (start < 0)
                start = 0;
            if (start >= total)
                start = total - size;

            // Para el repeater
            list = commentService.getComment(productId, start, size);

            this.SeeCommentListRepeater.DataSource = list;
            this.SeeCommentListRepeater.DataBind();

            

            this.pagination.Text = Pagination.showNextPrevious(start,
                                                               size,
                                                               total,
                                                               HttpContext.Current.Request.Url.AbsolutePath + "?id=" + productId + "&start={0}&size=" + size,
                                                               Response);
            
            if (aspXml.Visible)
                this.paginationTop.Text = this.pagination.Text;
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

        private bool herComment(long userId)
        {
            if (!this.authenticated)
                return false;

            return this.userId == userId;
        }

        protected void onItemDataBoundSeeComment(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = e.Item.ItemIndex;
                if (herComment(list[index].userProfileId))
                {
                    Panel pnl = e.Item.FindControl("pnlDeleteUpdate") as Panel;
                    pnl.Visible = true;

                    HyperLink delete = e.Item.FindControl("deleteComment") as HyperLink;
                    delete.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Comment/DeleteComment.aspx?id={0}", list[index].commentId));

                    HyperLink update = e.Item.FindControl("updateComment") as HyperLink;
                    update.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Comment/UpdateComment.aspx?id={0}", list[index].commentId));

                
                }
                Literal labels = e.Item.FindControl("tags") as Literal;
                if (list[index].Label.Count == 0)
                {
                    labels.Text = "<small>(" + GetLocalResourceObject("NoTags").ToString() + ")</small>";
                }
                else
                {
                    foreach (Model.Label tag in list[index].Label)
                    {
                        labels.Text += "<a href='" + Response.ApplyAppPathModifier(String.Format("~/Pages/Comment/SeeCommentByTag.aspx?id={0}", tag.labelId)) + "'>" +
                            "<span class='label label-primary'>" + tag.name + "</span></a>&#160;";
                    }
                }

            }
        }
    }
}