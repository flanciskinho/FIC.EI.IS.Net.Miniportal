using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.MiniPortal.Model.FavouriteService;
using Es.Udc.DotNet.MiniPortal.Web.Properties;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using System.Data;
using Es.Udc.DotNet.MiniPortal.Model;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;
using Es.Udc.DotNet.ModelUtil.Log;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.User
{
    public partial class ManageFavourite : System.Web.UI.Page
    {
        private long userId;

        private static IFavouriteService favouriteService;


        static ManageFavourite()
        {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            favouriteService = container.Resolve<IFavouriteService>();
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

            if (favouriteService.getNumberFavouriteByUserProfileId(userId) == 0)
            {
                this.pnlInfo.Visible = true;
                return;
            }

            if (!IsPostBack)
                BindData();
         


        }

        private void BindData()
        {
            int start, size;

            if (!Int32.TryParse(Request.Params.Get("start"), out start))
                start = 0;
            if (!Int32.TryParse(Request.Params.Get("size"), out size))
                size = Settings.Default.DefaultPageSize;

            int total = (int) favouriteService.getNumberFavouriteByUserProfileId(userId);
            if (size <= 0)
                size = Settings.Default.DefaultPageSize;
            if (start < 0)
                start = 0;
            if (start >= total)
                start = total - size;

            List<Favourite> list = favouriteService.getFavouriteByUserId(userId, start, size);

            this.gvFavourites.DataSource = list;
            this.gvFavourites.DataBind();

            
            //showNextPrevious(start, size, (int) favouriteService.getNumberFavouriteByUserProfileId(userId));
            this.pagination.Text = Pagination.showNextPrevious(start,
                                                               size,
                                                               total,
                                                               HttpContext.Current.Request.Url.AbsolutePath + "?start={0}&size=" + size,
                                                               Response);
        }

        //private void showNextPrevious(int start, int size, int total)
        //{
        //    if ((start - size) >= 0)
        //    {
        //        string url = HttpContext.Current.Request.Url.AbsolutePath +
        //            "?start=" + (start - size) +
        //            "&size=" + size;

        //        this.previous.NavigateUrl = Response.ApplyAppPathModifier(url);
        //        this.previous.Visible = true;
        //    }

        //    if (total > (start + size))
        //    {
        //        string url = HttpContext.Current.Request.Url.AbsolutePath +
        //            "?start=" + (start + size) +
        //            "&size=" + size;

        //        this.next.NavigateUrl = Response.ApplyAppPathModifier(url);
        //        this.next.Visible = true;
        //    }
        //}

        protected void DeleteRecord(object sender, GridViewDeleteEventArgs e)
        {
            long favId;
            
            string favIdStr = gvFavourites.DataKeys[e.RowIndex].Value.ToString();

            if (Int64.TryParse(favIdStr, out favId)) {
                try
                {
                    favouriteService.removeFromList(favId, userId);

                    /* Refresh data */
                    BindData();
                }
                catch (UnauthorizedException)
                {
                    this.pnlError.Visible = true;
                    this.lblError.Text = GetGlobalResourceObject("Common", "UnauthorizedUser").ToString();
                    this.gvFavourites.Visible = false;
                    this.pagination.Visible = false;
                }
            }
        }

    }
}