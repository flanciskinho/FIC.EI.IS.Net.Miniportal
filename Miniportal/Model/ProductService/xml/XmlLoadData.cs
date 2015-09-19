using System;
using System.Net;
using System.Text;
using System.Xml;
using Es.Udc.DotNet.MiniPortal.Model.Properties;
using System.Web;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;


namespace Es.Udc.DotNet.MiniPortal.Model.ProductService.xml
{
    public class XmlLoadData
    {

        private static StringBuilder getMinibayUrl(String page)
        {
            StringBuilder url = new StringBuilder();

            // anade la url especificada
            url.Append(Settings.Default.XmlRemoteHost);

            if (!Settings.Default.XmlRemoteHost.EndsWith("/"))
                url.Append("/");

            // anade la pagina
            url.Append(page);

            return url;
        }

        private static String getMinibayUrlList(string keywords, int start, int size)
        {
            StringBuilder url = getMinibayUrl(Settings.Default.XmlProductList);

            // anade el parametro de busqueda
            url.Append("?start=" + start);

            if (size <= 0)
                size = Settings.Default.DefaultPageSize;

            url.Append("&size="+size);

            url.Append("&keywords=" + keywords);

            return url.ToString();
        }

        private static String getMinibayUrlProduct(long productId)
        {
            StringBuilder url = getMinibayUrl(Settings.Default.XmlProduct);

            
            url.Append("?productId=" + productId);


            return url.ToString();
        }

        /// 
        /// Check the file exists or not.
        /// 
        /// The URL of the remote file.
        /// True : I)f the file exists, False if file not exists
        private static bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HTTPWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too
                request.Method = "HEAD";
                //Getting the Web Response
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                // Any exceptio nwill returns false
                return false;
            }

        }

        /// <exception cref="ServerNotRespondingException"/>
        public static XmlDocument getXmlList(string keywords, int start, int size)
        {
            string uri = XmlLoadData.getMinibayUrlList(keywords, start, size);


            // Comprobar si NO existe la url
            // Si no se cargara el fichero pero en local
            if (!XmlLoadData.RemoteFileExists(uri))
            {
                throw new ServerNotRespondingException();
                //uri = HttpContext.Current.Server.MapPath("~/xml/productList" + start + ".xml");//Esto es para test
            }


            // Carga el fichero xml
            XmlDocument doc = new XmlDocument();
            doc.Load(uri);

            return doc;
        }

        /// <exception cref="ServerNotRespondingException"/>
        public static XmlDocument getXmlProduct(long productId)
        {
            string uri = XmlLoadData.getMinibayUrlProduct(productId);

            // Comprobar si NO existe la url
            // Si no se cargara el fichero pero en local
            if (!XmlLoadData.RemoteFileExists(uri))
            {
                throw new ServerNotRespondingException();
                //uri = HttpContext.Current.Server.MapPath("~/xml/productDetails" + productId + ".xml");//Esto es para test
            }


            // Carga el fichero xml
            XmlDocument doc = new XmlDocument();
            doc.Load(uri);

            return doc;
        }
    }
}
