using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.MiniPortal.Model.UserService;
using Es.Udc.DotNet.MiniPortal.Model.FavouriteDao;
using Es.Udc.DotNet.MiniPortal.Model.FavouriteService;
using System.Transactions;
using Es.Udc.DotNet.MiniPortal.Model;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Test
{
    [TestClass]
    public class IFavouriteServiceTest
    {
        private static IUnityContainer container;

        private static IUserService userService;

        private static IFavouriteDao favouriteDao;
        private static IFavouriteService favouriteService;

        // Variables used in several test are initialized here
        private const String loginName = "login";
        private const String clearPass = "pass";
        private const String firstName = "name";
        private const String lastName = "lastname";
        private const String email = "user@udc.es";

        private const long NON_EXISTENT_USER_ID = -1;
        private const long NON_EXISTENT_FAVOURITE_ID = -1;
        private const long NON_EXISTENT_PRODUCT_ID = -1;

        private const long productId = 1;
        private const String name = "String for name";
        private const String comment = "String for comment";

        TransactionScope transaction;

        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run
        /// </summary>
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

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            container = TestManager.ConfigureUnityContainer("unity");

            userService = container.Resolve<IUserService>();
            favouriteService = container.Resolve<IFavouriteService>();
            favouriteDao = container.Resolve<IFavouriteDao>();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            TestManager.ClearUnityContainer(container);
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            transaction = new TransactionScope();
        }

        [TestCleanup()]
        public void MyTestCleanUp()
        {
            transaction.Dispose();
        }

        #endregion

        /// <exception cref="DuplicateInstanceException"/>
        private long createUserProfile()
        {
            UserProfileDetails userProfileDetails = new UserProfileDetails(firstName, lastName, email);
            return userService.RegisterUser(loginName, clearPass, userProfileDetails);
        }

        #region Test for addFavourite

        [TestMethod()]
        public void addFavouriteTest1()
        {
            long userProfileId = createUserProfile();
            long favouriteId = favouriteService.addFavourite(userProfileId, productId, name, comment);

            Favourite favourite = favouriteDao.Find(favouriteId);

            Assert.AreEqual(favouriteId, favourite.favouriteId);
            Assert.AreEqual(productId, favourite.productId);
            Assert.AreEqual(name, favourite.name);
            Assert.AreEqual(comment, favourite.comment);
        }

        [TestMethod()]
        public void addFavouriteTest2()
        {
            long userProfileId = createUserProfile();
            long favouriteId = favouriteService.addFavourite(userProfileId, productId, name, null);

            Favourite favourite = favouriteDao.Find(favouriteId);

            Assert.AreEqual(favouriteId, favourite.favouriteId);
            Assert.AreEqual(productId, favourite.productId);
            Assert.AreEqual(name, favourite.name);
            Assert.IsTrue(String.IsNullOrEmpty(favourite.comment));
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void addFavouriteNotFoundException()
        {
            favouriteService.addFavourite(NON_EXISTENT_USER_ID, productId, name, comment);
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void addFavouriteWithNoNameTest1()
        {
            long userProfileId = createUserProfile();
            long favouriteId = favouriteService.addFavourite(userProfileId, productId, null, comment);
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void addFavouriteWithNoNameTest2()
        {
            long userProfileId = createUserProfile();
            long favouriteId = favouriteService.addFavourite(userProfileId, productId, " ", comment);
        }

        #endregion

        #region Test for remove Favourite

        [TestMethod()]
        public void removeFavouriteTest()
        {
            long userProfileId = createUserProfile();
            long favouriteId = favouriteService.addFavourite(userProfileId, productId, name, comment);

            favouriteService.removeFromList(favouriteId, userProfileId);

            try
            {
                favouriteDao.Find(favouriteId);
                Assert.IsTrue(false);
            } catch (InstanceNotFoundException) {
                Assert.IsTrue(true);
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void removeNonExistentFavouriteTest()
        {
            favouriteService.removeFromList(NON_EXISTENT_FAVOURITE_ID, NON_EXISTENT_USER_ID);
        }

        #endregion

        #region Test for page by page

        [TestMethod()]
        public void getFavouriteNonExistentUserTest()
        {
            int cnt = 10;
            List<Favourite> list = favouriteService.getFavouriteByUserId(NON_EXISTENT_USER_ID, 0, cnt);
            long size = favouriteService.getNumberFavouriteByUserProfileId(NON_EXISTENT_USER_ID);

            Assert.AreEqual(list.Count, size);
            Assert.AreEqual(size, 0);
        }

        [TestMethod()]
        public void getFavouriteTest1()
        {
            int cnt = 10;

            long userProfileId = createUserProfile();
            long favouriteId = favouriteService.addFavourite(userProfileId, productId, name, comment);

            List<Favourite> list = favouriteService.getFavouriteByUserId(userProfileId, 0, cnt);
            long size = favouriteService.getNumberFavouriteByUserProfileId(userProfileId);

            Favourite favourite = favouriteDao.Find(favouriteId);

            Assert.AreEqual(list.Count, size);
            Assert.AreEqual(size, 1);
            Assert.IsTrue(list.Contains(favourite));
        }

        [TestMethod()]
        public void getFavouriteTest2()
        {
            List<Favourite> list;
            int cnt = 2;

            long userId = createUserProfile();

            long favouriteId1 = favouriteService.addFavourite(userId, productId+1, name, comment);
            System.Threading.Thread.Sleep(1001);
            long favouriteId2 = favouriteService.addFavourite(userId, productId+2, name, comment);
            System.Threading.Thread.Sleep(1001);
            long favouriteId3 = favouriteService.addFavourite(userId, productId+3, name, comment);

            long size = favouriteService.getNumberFavouriteByUserProfileId(userId);
            Assert.AreEqual(size, 3);

            list = favouriteService.getFavouriteByUserId(userId, 0, cnt);
            Assert.AreEqual(list.Count, cnt);
            Assert.IsTrue(list.Contains(favouriteDao.Find(favouriteId3)));
            Assert.IsTrue(list.Contains(favouriteDao.Find(favouriteId2)));
            //Assert.IsFalse(list.Contains(favouriteDao.Find(favouriteId1)));

            list = favouriteService.getFavouriteByUserId(userId, 2, cnt);
            Assert.AreEqual(list.Count, 1);
            //Assert.IsFalse(list.Contains(favouriteDao.Find(favouriteId3)));
            //Assert.IsFalse(list.Contains(favouriteDao.Find(favouriteId2)));
            Assert.IsTrue(list.Contains(favouriteDao.Find(favouriteId1)));
        }

        #endregion
    }
}
