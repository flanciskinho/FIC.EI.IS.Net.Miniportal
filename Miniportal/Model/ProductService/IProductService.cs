using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using System.Xml;
using System.Xml.XPath;


namespace Es.Udc.DotNet.MiniPortal.Model.ProductService
{
    public interface IProductService
    {

        /// <exception cref="ArgumentException" />
        /// <exception cref="ServerNotRespondingException" />
        List<Product> searchProductByKeywords(string keywords, int start, int size);

        /// <exception cref="ServerNotRespondingException" />
        XPathNavigator searchProductByKeywordsXML(string keywords, int start, int size);

        /// <exception cref="ArgumentException" />
        /// <exception cref="ServerNotRespondingException" />
        int getNumberOfProducts(string keyWords);

        /// <exception cref="ServerNotRespondingException" />
        XPathNavigator searchProductByIdXML(long productId);

        string getUrl2Bid(int productId);

        // /// <exception cref="ArgumentException" />
        // Product searchProductById(long productId);

        //List<Product> searchProducts(string keyWords, int start, int size);

        //Product searchProductDetails(long productId);

        //ProductDocument GetProductXmlDocument(string keyWords, int start, int size);

        //XmlDocument GetProductDetailsXmlDocument(long productId);
    }
}
