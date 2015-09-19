using System.Collections.Generic;
using System.Transactions;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

using Es.Udc.DotNet.MiniPortal.Model;
using Es.Udc.DotNet.MiniPortal.Model.ValuationDao;
using Es.Udc.DotNet.MiniPortal.Model.ValuationService;

using Es.Udc.DotNet.MiniPortal.Model.UserService;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;


namespace Es.Udc.DotNet.MiniPortal.Test
{
    /// <summary>
    /// Summary description for IValuationTest
    /// </summary>
    [TestClass]
    public class IValuationServiceTest
    {
        private static IUnityContainer container;

        private static IValuationService valuationService;
        private static IValuationDao valuationDao;

        private static IUserService userService;

        // Variables used in several test are initialized here
        private const String loginName = "login";
        private const String clearPass = "pass";
        private const String firstName = "name";
        private const String lastName = "lastname";
        private const String email = "user@udc.es";

        private const long NON_EXISTENT_USER_ID = -1;
        private const string NON_EXISTENT_SELLER_ID = "";

        private const string sellerId = "Pepito Pruebas";
        private const long productId = 1;
        private const long score = 3;
        private const String txt = "String's prove";


        TransactionScope transaction;

        #region Additional test attributes

        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            container = TestManager.ConfigureUnityContainer("unity");

            valuationService = container.Resolve<IValuationService>();
            valuationDao = container.Resolve<IValuationDao>();
            userService = container.Resolve<IUserService>();
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

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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


        /// <exception cref="DuplicateInstanceException"/>
        private long createUserProfile()
        {
            return createUserProfile(loginName);
        }

        /// <exception cref="DuplicateInstanceException"/>
        private long createUserProfile(string loginName)
        {
            UserProfileDetails userProfileDetails = new UserProfileDetails(firstName, lastName, email);
            return userService.RegisterUser(loginName, clearPass, userProfileDetails);
        }

        private long createValuation(long userProfileId, string text, long productId)
        {
            //return valuationService.addValuation(userProfileId, sellerId, productId, score, text);
            return createValuation(userProfileId, text, score, productId);
        }

        private long createValuation(long userProfileId, string text, long score, long productId)
        {
            return valuationService.addValuation(userProfileId, sellerId, productId, score, text);
        }

        [TestMethod()]
        public void getAverageTotalTest()
        {
            long score1 = 3;
            long score2 = 2;
            long userId = createUserProfile();
            long valuationId1 = createValuation(userId, txt + "1", score1, 1);
            long valuationId2 = createValuation(userId, txt + "2", score2, 2);

            double avg = valuationService.getAverageAndNumberOfValuations(sellerId).average;
            long total = valuationService.getAverageAndNumberOfValuations(sellerId).numberOfValuations;
            Assert.AreEqual(2, total);
            Assert.AreEqual((score1 + score2) / 2.0, avg);
        }

        #region test for add new valuation

        [TestMethod()]
        public void addValuationTest1()
        {
            long userId = createUserProfile();
            long valuationId = createValuation(userId, txt, productId);

            Valuation valuation = valuationDao.Find(valuationId);

            Assert.AreEqual(valuationId, valuation.valuationId);
            Assert.AreEqual(userId, valuation.userProfileId);
            Assert.AreEqual(sellerId, valuation.sellerId);
            Assert.AreEqual(productId, valuation.productId);
            Assert.AreEqual(txt, valuation.txt);
        }
        [TestMethod()]
        public void addValuationTest2()
        {
            long userId = createUserProfile();
            long valuationId = createValuation(userId, txt, productId);

            Valuation valuation = valuationService.getByUserIdAndProductId(userId, productId);

            Assert.AreEqual(valuationId, valuation.valuationId);
            Assert.AreEqual(userId, valuation.userProfileId);
            Assert.AreEqual(sellerId, valuation.sellerId);
            Assert.AreEqual(productId, valuation.productId);
            Assert.AreEqual(txt, valuation.txt);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void addValuationWithoutUserTest()
        {
            long valuationId = createValuation(NON_EXISTENT_USER_ID, txt, 1);
        }

        #endregion

        #region Test for page by page

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void getValuationForNonExistentSellerTest()
        {
            int cnt = 10;
            long size;
            List<Valuation> list;

            list = valuationService.listAllBySeller(NON_EXISTENT_SELLER_ID,0,cnt);
            Assert.AreEqual(list.Count, 0);

            //size = valuationService.getNumberOfValuations(NON_EXISTENT_SELLER_ID);
            size = valuationService.getAverageAndNumberOfValuations(NON_EXISTENT_SELLER_ID).numberOfValuations;
            Assert.AreEqual(size, 0);
        }

        [TestMethod()]
        public void testGetValuations1()
        {
            int cnt = 10;
            long size;
            List<Valuation> list;

            long userId = createUserProfile();
            long valuationId = createValuation(userId, txt, productId);
            

            Valuation valuation = valuationDao.Find(valuationId);


            list = valuationService.listAllBySeller(sellerId, 0, cnt);
            //size = valuationService.getNumberOfValuations(sellerId);
            size = valuationService.getAverageAndNumberOfValuations(sellerId).numberOfValuations;

            Assert.AreEqual(list.Count, size);
            Assert.AreEqual(size, 1);
        }

        [TestMethod()]
        public void testGetValuations2()
        {
            int cnt = 2;
            long size;
            List<Valuation> list;

            long userId1 = createUserProfile(loginName+"1");
            long userId2 = createUserProfile(loginName+"2");

            long valuationId1 = createValuation(userId1, txt + "1", 1);
            System.Threading.Thread.Sleep(1001);
            long valuationId2 = createValuation(userId2, txt + "2", 1);
            System.Threading.Thread.Sleep(1001);
            long valuationId3 = createValuation(userId1, txt + "3", 2);

            Valuation v1 = valuationDao.Find(valuationId1);
            Valuation v2 = valuationDao.Find(valuationId2);
            Valuation v3 = valuationDao.Find(valuationId3);
            
            //size = valuationService.getNumberOfValuations(sellerId);
            size = valuationService.getAverageAndNumberOfValuations(sellerId).numberOfValuations;
            Assert.AreEqual(size, 3);

            list = valuationService.listAllBySeller(sellerId, 0, cnt);
            Assert.AreEqual(list.Count, 2);

            list = valuationService.listAllBySeller(sellerId, 2, cnt);
            Assert.AreEqual(list.Count, 1);

            Assert.AreEqual(valuationId1, v1.valuationId);
            Assert.AreEqual(valuationId2, v2.valuationId);
            Assert.AreEqual(valuationId2, v2.valuationId);
            
            
        }

        #endregion


        #region prove get avg and count from list of sellers

        [TestMethod]
        public void testGetAvgCntBySellers1() {
            long userId1 = createUserProfile(loginName + "1");

            long valuationId1 = createValuation(userId1, txt + "1", 1, 1);
            //System.Threading.Thread.Sleep(1001);
            long valuationId2 = createValuation(userId1, txt + "2", 2, 2);
            //System.Threading.Thread.Sleep(1001);
            long valuationId3 = createValuation(userId1, txt + "3", 3, 3);

            //AverageAndNumberOfValuations tmp = valuationService.getAverageAndNumberOfValuations(sellerId);
            //Assert.AreEqual(tmp.numberOfValuations, 3);

            List<string> sellers = new List<string>();
            sellers.Add(sellerId);

            Dictionary<string, AverageAndNumberOfValuations> list;
            list = valuationService.getAverageAndNumberOfValuations(sellers);

            Assert.AreEqual(list.Count, 1);
            Assert.AreEqual(list[sellerId].numberOfValuations, 3);
            Assert.AreEqual(list[sellerId].average, (1.0 + 2.0 + 3.0) / 3.0);

            //Add the same seller again
            sellers.Add(sellerId);
            list = valuationService.getAverageAndNumberOfValuations(sellers);

            Assert.AreEqual(list.Count, 1);

            try
            {
                Assert.AreEqual(list["Null"].numberOfValuations, 10);
                Assert.IsFalse(true);
            }
            catch (KeyNotFoundException)
            {
                Assert.IsTrue(true);
            }
        }


        [TestMethod]
        public void testGetAvgCntBySellers2() {
            long userId = createUserProfile(loginName);
            string seller1 = "s1";
            string seller2 = "seller2";

            long s1v1 = valuationService.addValuation(userId, seller1, 10, 1, "");
            long s1v2 = valuationService.addValuation(userId, seller1, 20, 2, "");
            long s1v3 = valuationService.addValuation(userId, seller1, 30, 4, "");

            long s2v1 = valuationService.addValuation(userId, seller2, 40, 4, "");
            long s2v2 = valuationService.addValuation(userId, seller2, 50, 5, "");


            List<string> seller = new List<string>();
            seller.Add(seller1);
            seller.Add(seller2);
            seller.Add(seller2);
            seller.Add(seller1);
            seller.Add(seller2);
            seller.Add(seller1);
            seller.Add("NotFound");

            Dictionary<string, AverageAndNumberOfValuations> dic;
            dic = valuationService.getAverageAndNumberOfValuations(seller);

            Assert.AreEqual(dic.Count, 2);

            //seller1
            Assert.AreEqual(dic[seller1].numberOfValuations, 3);
            Assert.AreEqual(dic[seller1].average, (1.0 + 2.0 + 4.0) / 3.0);

            //seller2
            Assert.AreEqual(dic[seller2].numberOfValuations, 2);
            Assert.AreEqual(dic[seller2].average, (4.0 + 5.0) / 2.0);
        }

        #endregion
    }
}
