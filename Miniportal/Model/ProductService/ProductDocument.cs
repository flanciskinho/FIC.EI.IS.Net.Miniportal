using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Es.Udc.DotNet.MiniPortal.Model.ProductService
{
    public class ProductDocument
    {
        public XmlDocument document { get; set; }
        public int numberOfProducts { get; set; }

        public ProductDocument(XmlDocument doc, int number)
        {
            document = doc;
            numberOfProducts = number;
        }
    }
}
