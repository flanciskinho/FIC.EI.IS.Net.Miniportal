using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Es.Udc.DotNet.MiniPortal.Model.ProductService
{
    public class Product
    {
        public long productId {get; set;}
        public string productName { get; set; }
        public string sellerId { get; set; }
        public string category { get; set; }
        public double currentPrice { get; set; }
        public long minutesToEnd { get; set; }
        public double average { get; set;}
        public long numberOfValuation { get; set; }

        public Product()
        {
        }

        public Product(long productId, string name, string seller, string category, double prize, long minutes)
        {
            this.productId = productId;
            this.sellerId = seller;
            this.productName = name;
            this.category = category;
            this.currentPrice = prize;
            this.minutesToEnd = minutes;
            // Los dejamos a cero por si algun vendedor no tuvo aún valoraciones
            this.average = 0;
            this.numberOfValuation = 0;
        }

        public Product(long productId, string name, string seller, string category, double prize, long minutes, double average, long number)
        {
            this.productId = productId;
            this.sellerId = seller;
            this.productName = name;
            this.category = category;
            this.currentPrice = prize;
            this.minutesToEnd = minutes;
            this.average = average;
            this.numberOfValuation = number;
        }

        public override string ToString()
        {
            return "[total:" + numberOfValuation + ", avg:" + average + ", id:" + productId + ", name:" + productName + ", seller:" + sellerId + "]";
        }
    }
}
