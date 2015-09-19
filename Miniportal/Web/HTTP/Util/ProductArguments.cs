using System.Collections.Generic;

namespace Es.Udc.DotNet.MiniPortal.Web.HTTP.Util
{
    public class ProductArguments
    {
        //Argumentos para la lista de productos (para el xslt)
        private static List<string> paramProductDetails;

        //Argumentos para el detalle de productos (para el xslt)
        private static List<string> paramProductList;

        static ProductArguments()
        {
            paramProductList = new List<string>();
            paramProductList.Add("Name");
            paramProductList.Add("Seller");
            paramProductList.Add("Category");
            paramProductList.Add("Price");
            paramProductList.Add("Minutes2End");
            paramProductList.Add("min");

            paramProductDetails = new List<string>();
            paramProductDetails.Add("Product");
            paramProductDetails.Add("Seller");
            paramProductDetails.Add("Category");
            paramProductDetails.Add("Price");
            paramProductDetails.Add("Minutes2End");
        }


        public static List<string> getParamProductDetails()
        {
            return paramProductDetails;
        }

        public static List<string> getParamProductList()
        {
            return paramProductList;
        }
    }

}