using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.MiniPortal.Model.ValuationService;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Web.UI.HtmlControls;
using Es.Udc.DotNet.MiniPortal.Web.Properties;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Util;

namespace Es.Udc.DotNet.MiniPortal.Web.Pages.Valuation
{
    public partial class SeeValuation : System.Web.UI.Page
    {

        private string sellerId;
        private int start;
        private int size;

        private List<Model.Valuation> list;

        private AverageAndNumberOfValuations avgTotal;

        private static IValuationService valuationService;


        static SeeValuation()
        {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            valuationService   = container.Resolve<IValuationService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = GetLocalResourceObject("title").ToString();

            sellerId = Request.Params.Get("seller");

            if (String.IsNullOrEmpty("seller"))
            {
                this.pnlNotFound.Visible = true;
                return;
            }

            if (!Int32.TryParse(Request.Params.Get("start"), out start))
                start = 0;
            if (!Int32.TryParse(Request.Params.Get("size"), out size))
                size = Settings.Default.DefaultPageSize;

            this.pnlValuation.Visible = true;

            this.lblSeller.Text = sellerId;

            try
            {
                avgTotal = valuationService.getAverageAndNumberOfValuations(sellerId);
                lblValuation.Text = getNameValuation((int)avgTotal.average) + " &nbsp; (" + avgTotal.numberOfValuations+ " ";
                lblValuation.Text += (avgTotal.numberOfValuations == 1) ?
                        GetLocalResourceObject("OneAssessment").ToString() :
                        GetLocalResourceObject("MoreAssesment").ToString();
                lblValuation.Text += ")";
            }
            catch (InstanceNotFoundException) {
                avgTotal.average = avgTotal.numberOfValuations = 0;
                lblValuation.Text = "<i>" + GetLocalResourceObject("NoAssessment").ToString() + "</i>";
                return;
            }
            if (size <= 0)
                size = Settings.Default.DefaultPageSize;
            if (start < 0)
                start = 0;
            if (start >= avgTotal.numberOfValuations)
                start = (int) avgTotal.numberOfValuations - size;

            list = valuationService.listAllBySeller(sellerId, start, size);

            this.ValuationListRepeater.DataSource = list;
            this.ValuationListRepeater.DataBind();

            //showNextPrevious(start, size, (int) avgTotal.numberOfValuations);

            this.pagination.Text = Pagination.showNextPrevious(start,
                                                               size,
                                                                (int)avgTotal.numberOfValuations,
                                                               HttpContext.Current.Request.Url.AbsolutePath + "?seller="+sellerId+"&start={0}&size=" + size,
                                                               Response);
        }

        //private void showNextPrevious(int start, int size, int total)
        //{
        //    if ((start - size) >= 0)
        //    {
        //        string url = HttpContext.Current.Request.Url.AbsolutePath +
        //            "?seller=" + sellerId +
        //            "&start=" + (start - size) +
        //            "&size=" + size;

        //        this.previous.NavigateUrl = Response.ApplyAppPathModifier(url);
        //        this.previous.Visible = true;
        //    }

        //    if (total > (start + size))
        //    {
        //        string url = HttpContext.Current.Request.Url.AbsolutePath +
        //            "?seller=" + sellerId +
        //            "&start=" + (start + size) +
        //            "&size=" + size;

        //        this.next.NavigateUrl = Response.ApplyAppPathModifier(url);
        //        this.next.Visible = true;
        //    }
        //}

        protected void onItemDataBoundValuation(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal score = e.Item.FindControl("score") as Literal;
                score.Text = getNameValuation((int) list[e.Item.ItemIndex].score) ;
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