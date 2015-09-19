using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Globalization;


using Es.Udc.DotNet.MiniPortal.Model.ProductService.xml;
using Es.Udc.DotNet.MiniPortal.Model.Properties;
using System.Text;
using Es.Udc.DotNet.MiniPortal.Model.ValuationService;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.ModelUtil.Log;

namespace Es.Udc.DotNet.MiniPortal.Model.ProductService
{
   public class ProductService : IProductService
    {

       [Dependency]
       public IValuationService valuationService { private get; set; }

       /// <exception cref="ArgumentException" />
       /// <exception cref="ServerNotRespondingException" />
       public List<Product> searchProductByKeywords(string keywords, int start, int size)
       {
            List<Product> productList = new List<Product>();
            List<string> sellerList = new List<string>();

            XmlDocument doc = XmlLoadData.getXmlList(keywords, start, size);

            XmlNodeList nodes = doc.GetElementsByTagName("product");

            foreach (XmlNode node in nodes)
            {
                // Parsear el id
                Int64 productId;
                if (!Int64.TryParse(node.Attributes[6].Value, out productId)) {
                    throw new ArgumentException("Product Id is not a valid number", node.Attributes[6].Value);
                }


                // Parsear el precio
                Double prize;
                if (!Double.TryParse(node.Attributes[0].Value, out prize)) {
                    throw new ArgumentException("Prize is not a valid number", node.Attributes[0].Value);
                }

                // Parsear los minutos
                Int64 minutes;
                if (!Int64.TryParse(node.Attributes[1].Value, out minutes)) {
                    throw new ArgumentException("MinutesToEnd is not a valid number", node.Attributes[1].Value);
                }


                productList.Add(new Product(
                    productId,
                    node.Attributes[4].Value,
                    node.Attributes[3].Value,
                    node.Attributes[5].Value,
                    prize,
                    minutes
                    ));
                sellerList.Add(node.Attributes[3].Value);
            }

            // Ahora hay que rellenar el productList con todas las estadisticas
            Dictionary<string, AverageAndNumberOfValuations> dic = valuationService.getAverageAndNumberOfValuations(sellerList);


            foreach (Product p in productList)
            {
                try
                {
                    AverageAndNumberOfValuations anv = dic[p.sellerId];
                    p.numberOfValuation = anv.numberOfValuations;
                    p.average = anv.average;
                }
                catch (KeyNotFoundException)
                {
                }
            }
            return productList;
        }


       /// <exception cref="ServerNotRespondingException" />
       public XPathNavigator searchProductByKeywordsXML(string keywords, int start, int size)
       {
           XmlDocument doc = XmlLoadData.getXmlList(keywords, start, size);

           return doc.CreateNavigator();
       }

       /// <exception cref="ArgumentException" />
       /// <exception cref="ServerNotRespondingException" />
       public int getNumberOfProducts(string keyWords)
        {
           // Decimos que solo traiga uno para que tenga que traer menos datos del servidor
           // También se podría poner que en el metodo que devulve la lista uno de los parámetros de la lista sea (...,...,..., out int total)
            XmlDocument doc = XmlLoadData.getXmlList(keyWords, 0, 1);

            //XmlNode node = doc.GetElementById("products");
            XmlNode node = doc.DocumentElement;

            int number;
            if (!Int32.TryParse(node.Attributes[1].Value, out number))
            {
                throw new ArgumentException("Total has no a valid value.", node.Attributes[1].Value);
            }

            return number;
        }

       /// <exception cref="ServerNotRespondingException" />
       public XPathNavigator searchProductByIdXML(long productId)
       {
           XmlDocument doc = XmlLoadData.getXmlProduct(productId);

           return doc.CreateNavigator();
       }

       public string getUrl2Bid(int productId)
       {
           StringBuilder url = new StringBuilder();

           // anade la url especificada
           url.Append(Settings.Default.XmlRemoteHost);

           if (!Settings.Default.XmlRemoteHost.EndsWith("/"))
               url.Append("/");

           // anade la pagina
           url.Append(Settings.Default.BidRemoteHost);
           if (!Settings.Default.XmlRemoteHost.EndsWith("/"))
               url.Append("/");

           // anadimos el producto
           url.Append(productId);

           return url.ToString();
       }

       ///// <exception cref="ArgumentException" />
       // public Product searchProductById(long productId)
       // {
       //     Product product = new Product();

       //     XmlDocument doc = XmlLoadData.getXmlProduct(productId);

       //     XmlNode node = doc.GetElementById("details");

       //     String name = node.Attributes[8].Value;
       //     String category = node.Attributes[7].Value;
       //     String seller = node.Attributes[6].Value;


       //     DateTime myDateTime;
       //     if (!DateTime.TryParseExact(node.Attributes[4].Value, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out myDateTime))
       //     {
       //         throw new ArgumentException("Ends is not a valid datetime", node.Attributes[2].Value);
       //     }

       //     // Parsear el precio
       //     Double prize;
       //     if (!Double.TryParse(node.Attributes[0].Value, out prize))
       //     {
       //         throw new ArgumentException("Prize is not a valid number", node.Attributes[0].Value);
       //     }

       //     Int64 minutes = (Int64)(myDateTime - DateTime.Now).TotalMinutes; //Esto fijo que esta mal


       //     AverageAndNumberOfValuations anv = valuationService.getAverageAndNumberOfValuations(seller);

       //     return new Product(productId, name, seller, category, prize, minutes, anv.average, anv.numberOfValuations);
       // }

        //public List<Product> searchProducts(string keyWords, int start, int size)
        //{
        //    ProductDocument productDocument = GetProductXmlDocument(keyWords, start, size);
        //    XmlDocument document = productDocument.document;
        //    int numberOfProducts = productDocument.numberOfProducts;
        //    XPathNavigator navigator = document.CreateNavigator();
        //    XPathNodeIterator iterator;

        //    iterator = navigator.Select("//product");
        //    Product product;
        //    List<Product> list = new List<Product>();

        //    while (iterator.MoveNext())
        //    {
        //        product = new Product();
                
        //        iterator.Current.MoveToAttribute("id", "");
        //        product.productId = Convert.ToInt64(iterator.Current.Value);
        //        iterator.Current.MoveToParent();

        //        iterator.Current.MoveToAttribute("category", "");
        //        product.category = iterator.Current.Value;
        //        iterator.Current.MoveToParent();

        //        iterator.Current.MoveToAttribute("name", "");
        //        product.productName = iterator.Current.Value;
        //        iterator.Current.MoveToParent();

        //        iterator.Current.MoveToAttribute("ends", "");
        //        product.endBid = DateTime.Parse(iterator.Current.Value);
        //        iterator.Current.MoveToParent();

        //        iterator.Current.MoveToAttribute("price", "");
        //        product.currentPrice = double.Parse(iterator.Current.Value);
        //        iterator.Current.MoveToParent();

        //        list.Add(product);
        //    }

        //    return list;
        //}

        //public Product searchProductDetails(long productId)
        //{

        //    XmlDocument document = GetProductDetailsXmlDocument(productId);
        //    XPathNavigator navigator = document.CreateNavigator();
        //    XPathNodeIterator iterator = navigator.Select("//details");

        //    Product product = new Product();

        //    product.productId = productId;

        //    iterator.Current.MoveToAttribute("category", "");
        //    product.category = iterator.Current.Value;
        //    iterator.Current.MoveToParent();

        //    iterator.Current.MoveToAttribute("name", "");
        //    product.productName = iterator.Current.Value;
        //    iterator.Current.MoveToParent();

        //    iterator.Current.MoveToAttribute("endDate","");
        //    product.endBid = DateTime.Parse(iterator.Current.Value);
        //    iterator.Current.MoveToParent();

        //    iterator.Current.MoveToAttribute("currentPrice","");
        //    product.currentPrice = double.Parse(iterator.Current.Value, CultureInfo.InvariantCulture);


        //    return product;

        //}
   
        //public ProductDocument GetProductXmlDocument(string keyWords, int start, int size)
        //{
        //    string uri = "http://localhost:8080/minibay/products/XmlListProducts" + "?" + "start=" + start + "&" + "size=" + size + "&" + "keywords=" + keyWords;

        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.Load(uri);
        //    XPathNavigator navigator = xmlDocument.CreateNavigator();
        //    XPathNodeIterator iterator = navigator.Select("//products");

        //    iterator.Current.MoveToAttribute("total","");
        //    int numberOfProducts = Int32.Parse(iterator.Current.Value);             //Dice que no tiene formato correcto...

        //    return new ProductDocument(xmlDocument, numberOfProducts);
        //}

        //public XmlDocument GetProductDetailsXmlDocument(long productId)
        //{
        //    string uri = "http://localhost:8080/minibay/products/xmlproductdetails" + "?" +
        //                 "productid" + "=" + productId;

        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.Load(uri);


        //    return xmlDocument;
        //}

        //Cosa rara para integrar resultados con votos. (parte optativa)
    }
}
