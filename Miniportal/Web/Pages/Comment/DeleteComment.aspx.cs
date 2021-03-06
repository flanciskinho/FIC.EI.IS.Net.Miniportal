﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Xml.Xsl;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;
using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.Comment
{
    public partial class DeleteComment : System.Web.UI.Page
    {
        private long userId;
        private long commentId;


        private Model.Comment comm;

        private static ICommentService commentService;
        private static IProductService productService;


        static DeleteComment()
        {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            productService   = container.Resolve<IProductService>();
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
            if (!Int64.TryParse(Request.Params.Get("id"), out commentId))
            {
                //this.aspXml.Visible = this.pnlInfo.Visible = this.pnlError.Visible = this.pnlForm.Visible = this.pnlAddRight.Visible= false;
                this.pnlNotFound.Visible = true;
                return;
            }

            
            try
            {
                comm = commentService.getCommentById(commentId);
            }
            catch (InstanceNotFoundException)
            {
                this.pnlNotFound.Visible = true;
                return;
            }

            if (comm.userProfileId != userId)
            {
                this.pnlDenyPermission.Visible = true;
                return;
            }


            try
            {
                aspXml.Visible = true;
                //Traemos los productos
                aspXml.XPathNavigator = productService.searchProductByIdXML(comm.productId);

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
            

            this.pnlComment.Visible = true;
            this.userName.Text = comm.UserProfile.loginName;
            this.addDate.Text  = comm.addDate.ToString();
            this.comment.Text  = comm.txt;



            if (comm.Label.Count == 0)
            {
                this.tags.Text = "<small>("+GetLocalResourceObject("NoTags").ToString()+")</small>";
            }
            else
            {
                foreach (Model.Label tag in comm.Label)
                {
                    this.tags.Text += "<a href='" + Response.ApplyAppPathModifier(String.Format("~/Pages/Comment/SeeCommentByTag.aspx?id={0}", tag.labelId)) + "'>" +
                    "<span class='label label-primary'>" + tag.name + "</span></a>&#160;";
                }
            }
        }

        private XsltArgumentList CreateArgumentListAndParams()
        {
            XsltArgumentList args = new XsltArgumentList();

            foreach (string str in ProductArguments.getParamProductDetails())
                args.AddParam(str, "", GetGlobalResourceObject("Common", str).ToString());


            args.AddParam("ProductId", "", comm.productId);

            args.AddExtensionObject("ext:ProductList", commentService);

            return args;
        }

        protected void BtnDeleteClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    commentService.removeComment(userId, commentId);

                    this.pnlComment.Visible = false;
                    this.pnlDeleteSuccess.Visible = true;
                }
                catch (InstanceNotFoundException)
                {
                    this.pnlComment.Visible = false;
                    this.pnlNotFound.Visible = true;
                }
                catch (Model.Exceptions.UnauthorizedException)
                {
                    this.pnlComment.Visible = false;
                    this.pnlDenyPermission.Visible = true;
                }
            }

        }
    }
}