using System;
using System.Collections.Generic;

using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using Es.Udc.DotNet.MiniPortal.Web.HTTP.Session;
using Es.Udc.DotNet.MiniPortal.Web.Properties;
using Microsoft.Practices.Unity;
using System.Web;


namespace Es.Udc.DotNet.MiniPortal.Web.Pages
{

    public partial class MainPage : SpecificCulturePage
    {
        private static IProductService productService;

        static MainPage()
        {
            IUnityContainer container =
             (IUnityContainer)HttpContext.Current.Application["unityContainer"];

            productService   = container.Resolve<IProductService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void showStatistics_OnClick(Object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string text = this.searchBar.Text;
                string url = String.Format("~/Pages/Product/ProductList.aspx?keyword={0}&start={1}&size={2}&stat=1", this.searchBar.Text, 0, Settings.Default.DefaultPageSize);

                Response.Redirect(Response.ApplyAppPathModifier(url));
            }
        }

        protected void searchButton_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                String text = this.searchBar.Text;

                string url = String.Format("~/Pages/Product/ProductList.aspx?keyword={0}&start={1}&size={2}", this.searchBar.Text, 0, Settings.Default.DefaultPageSize);

                Response.Redirect(Response.ApplyAppPathModifier(url));
            }
        }

    }
}
