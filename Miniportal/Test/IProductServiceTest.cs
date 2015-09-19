using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

using Es.Udc.DotNet.MiniPortal.Model;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;
using System.Transactions;
using Es.Udc.DotNet.MiniPortal.Model.ProductService;
using System.Xml.XPath;

namespace Es.Udc.DotNet.MiniPortal.Test
{
    /// <summary>
    /// Descripción resumida de IProductTest
    /// </summary>
    [TestClass]
    public class IProductServiceTest
    {

        private TestContext testContextInstance;
        private static IUnityContainer container;
        TransactionScope transaction;
        private static IProductService productService;

        private static String keyWord = "placa";
       // private static String NON_EXISTENT_KEYWORD = "arduino";
        private static int start = 0;
        private static int size = 10;

        /// <summary>
        ///Obtiene o establece el contexto de las pruebas que proporciona
        ///información y funcionalidad para la ejecución de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            container = TestManager.ConfigureUnityContainer("unity");

            productService = container.Resolve<IProductService>();

        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            TestManager.ClearUnityContainer(container);
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            transaction = new TransactionScope();
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            transaction.Dispose();
        }

        #endregion

        /*
         * Problema en HttpContext.Current.Server */
        [TestMethod()]
        public void TestSearchProductByKeyword()
        {
           // List<Product> list = productService.searchProductByKeywords("a", 0, 10);

        }
        


        [TestMethod()]
        public void TestSearchProducts()
        {
        }
    }
}
