using System;

using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.MiniPortal.Web.Properties;
using Es.Udc.DotNet.MiniPortal.Model;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using Microsoft.Practices.Unity;
using System.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.MiniPortal.Web
{

    public partial class Miniportal : System.Web.UI.MasterPage
    {
        private List<Model.Label> labels;
        private long cntOfLabelsMoreUsed;

        public static readonly String USER_SESSION_ATTRIBUTE = "userSession";
        private static ICommentService commentService;
        protected static int maxLabels;

        public Miniportal()
        {
            IUnityContainer container = (IUnityContainer)HttpContext.Current.Application["unityContainer"];
            commentService = container.Resolve<ICommentService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!SessionManager.IsUserAuthenticated(Context))
            {

                if (lblDash2 != null)
                    lblDash2.Visible = false;
                if (lnkUpdate != null)
                    lnkUpdate.Visible = false;
                if (lblDash3 != null)
                    lblDash3.Visible = false;
                if (lnkLogout != null)
                    lnkLogout.Visible = false;
                if (lnkFavourite != null)
                    lnkFavourite.Visible = false;

            }
            else
            {
                if (lblWelcome != null)                   
                    lblWelcome.Text =
                        GetLocalResourceObject("lblWelcome.Hello.Text").ToString()
                        + " " + SessionManager.GetUserSession(Context).FirstName;
                if (lblDash1 != null)
                    lblDash1.Visible = false;
                if (lnkAuthenticate != null)
                    lnkAuthenticate.Visible = false;
            }

            try
            {
                maxLabels = Settings.Default.maxLabels;
                labels = commentService.getLabels(0, maxLabels);
                if (labels.Count != 0)
                {
                    cntOfLabelsMoreUsed = 0;
                    foreach (Model.Label tag in labels)
                    {
                        cntOfLabelsMoreUsed += tag.cnt;
                    }

                    this.pnlWordCloud.Visible = true;
                    this.WordCloudRepeater.DataSource = labels;
                    this.WordCloudRepeater.DataBind();
                }

            }
            catch { }
            
        }


        protected void onItemDataBoundWordCloud(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int index = e.Item.ItemIndex;

                HyperLink tmp = e.Item.FindControl("cloudLink") as HyperLink;
                tmp.NavigateUrl = Response.ApplyAppPathModifier(String.Format("~/Pages/Comment/SeeCommentByTag.aspx?id={0}", labels[index].labelId));

                long cnt  = labels[index].cnt;
                if (cnt > 0.35 * cntOfLabelsMoreUsed)
                {
                    tmp.CssClass += " btn-large btn-danger";
                }
                else if (cnt > 0.2 * cntOfLabelsMoreUsed)
                {
                    tmp.CssClass += " btn-warning";
                }
                else if (cnt > 0.15 * cntOfLabelsMoreUsed)
                {
                    tmp.CssClass += " btn-small btn-success";
                }
                else if (cnt > 0.1 * cntOfLabelsMoreUsed)
                {
                    tmp.CssClass += " btn-mini btn-info";
                }
                else if (cnt != 0)
                {
                    tmp.CssClass += " btn-mini btn-inverse";
                }
                else
                {
                    tmp.Visible = false;
                }
            }
        }
    }
}
