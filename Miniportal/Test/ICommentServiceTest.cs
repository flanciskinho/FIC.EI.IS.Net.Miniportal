using System.Collections.Generic;
using System.Transactions;
using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.UserService;
using Es.Udc.DotNet.MiniPortal.Model.CommentService;
using Es.Udc.DotNet.MiniPortal.Model.CommentDao;
using Es.Udc.DotNet.MiniPortal.Model.LabelDao;
using Es.Udc.DotNet.MiniPortal.Model;
using Es.Udc.DotNet.MiniPortal.Model.CommentService.Util;

using System.Data.Objects.DataClasses;

namespace Es.Udc.DotNet.MiniPortal.Test
{
    [TestClass]
    public class ICommentServiceTest
    {
        private static IUnityContainer container;

        private static IUserService userService;

        private static ICommentDao commentDao;
        private static ICommentService commentService;

        private static ILabelDao labelDao;

        // Variables used in several test are initialized here
        private const String loginName = "login";
        private const String clearPass = "pass";
        private const String firstName = "name";
        private const String lastName = "lastname";
        private const String email = "user@udc.es";

        private const long NON_EXISTENT_USER_ID = -1;
        private const long NON_EXISTENT_COMMENT_ID = -1;
        private const long NON_EXISTENT_PRODUCT_ID = -1;

        private const long productId = 1;
        private const string txt = "String's prove";
        private static List<string> labels = new List<string>();

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
            commentService = container.Resolve<ICommentService>();
            commentDao = container.Resolve<ICommentDao>();
            labelDao = container.Resolve<ILabelDao>();

            string[] tmp = new string[] { "Opinión Personal", "CARACTERíSTiCAS TéCNiCAS", "Tecnología" };
            for (int cnt = 0; cnt < tmp.Length; cnt++)
            {
                labels.Add(tmp[cnt]);
            }
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
            return createUserProfile(loginName);
        }

        /// <exception cref="DuplicateInstanceException"/>
        private long createUserProfile(string loginName)
        {
            UserProfileDetails userProfileDetails = new UserProfileDetails(firstName, lastName, email);
            return userService.RegisterUser(loginName, clearPass, userProfileDetails);
        }

        private long createComment(long userProfileId, String txt)
        {
            return createComment(userProfileId, txt, null);
        }

        private long createComment(long userProfileId, String txt, List<string> labelNames)
        {
            return commentService.addComment(userProfileId, productId, txt, labelNames);
        }


        private void createLabel(long commentId, List<String> labelList, long userProfileId)
        {
            commentService.addLabels(commentId, labelList, userProfileId);
        }

        /// <exception cref="InstanceNotFoundException"/>
        private bool checkMatchLabels(List<string> list, EntityCollection<Label> labels)
        {
            foreach (string str in list)
            {
                Label l = labelDao.findByName(StrNormalize.strNormalize(str));//labelDao.findByName(StrRegression.removeAccents(str).ToLower());

                if (!labels.Contains(l))
                    return false;
            }

            return true;
        }

        #region test for add new comment

        [TestMethod()]
        public void addCommentTest()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);

            Comment comment = commentDao.Find(commentId);

            Assert.AreEqual(commentId, comment.commentId);
            Assert.AreEqual(productId, comment.productId);
            Assert.AreEqual(txt, comment.txt);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void addCommentWithoutUserTest()
        {
            long commentId = createComment(NON_EXISTENT_USER_ID, txt);
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void addCommentWithoutTextTest1()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, null);
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void addCommentWithoutTextTest2()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, "");
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void addCommentWithoutTextTest3()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, " ");
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void addCommentWithoutTextTest4()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, " \t");
        }

        public void TextGetCommetById()
        {
            long userId = createUserProfile();
            long commentId = createComment(userId, "Comentario");

            Comment c = commentService.getCommentById(commentId);

            Assert.AreEqual("Comentario", c.txt);
            Assert.AreEqual(userId, c.userProfileId);
        }


        #endregion

        #region Test for modify Comment

        [TestMethod()]
        public void modifyCommentTest()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);

            String str = "New string";
            commentService.modifyComment(commentId, str, userProfileId,null);

            Comment comment = commentDao.Find(commentId);

            Assert.AreEqual(commentId, comment.commentId);
            Assert.AreEqual(productId, comment.productId);
            Assert.AreEqual(userProfileId, comment.userProfileId);
            Assert.AreEqual(str, comment.txt);
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void modifyCommentWithoutTextTest1()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);

            commentService.modifyComment(commentId, null, userProfileId, null);
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void modifyCommentWithoutTextTest2()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);

            commentService.modifyComment(commentId, " ", userProfileId, null);
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void modifyCommentWithoutTextTest3()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);

            commentService.modifyComment(commentId, " \t", userProfileId, null);
        }

        [TestMethod()]
        [ExpectedException(typeof(EmptyStringException))]
        public void modifyCommentWithoutTextTest4()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);

            commentService.modifyComment(commentId, "", userProfileId,null);
        }

        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void modifyNonExistentCommentTest()
        {
            commentService.modifyComment(NON_EXISTENT_COMMENT_ID, txt, NON_EXISTENT_USER_ID,null);
        }

        [TestMethod()]
        public void modifyLabelCommentTest()
        {
            long userId = createUserProfile();

            List<String> oldLabels = new List<String>();
            List<String> newLabels = new List<String>();

            oldLabels.Add("label1");    oldLabels.Add("label2");
            newLabels.Add("label2");    newLabels.Add("label3");

            long commentId = createComment(userId,"comentario",oldLabels);

            commentService.modifyComment(commentId,"comentario",userId,newLabels);

            Label label1 = labelDao.findByName("label1");
            Label label2 = labelDao.findByName("label2");
            Label label3 = labelDao.findByName("label3");

            Assert.AreEqual(0,label1.cnt);
            Assert.AreEqual(1,label2.cnt);
            Assert.AreEqual(1,label3.cnt);

        }
        #endregion


        #region Test for page by page

        [TestMethod()]
        public void getCommentsForNonExistentProductTest()
        {
            int cnt = 10;
            long size;
            List<Comment> list;

            list = commentService.getComment(NON_EXISTENT_PRODUCT_ID, 0, cnt);
            size = commentService.getNumberOfComment(NON_EXISTENT_PRODUCT_ID);
            Assert.AreEqual(list.Count, size);
            Assert.AreEqual(size, 0);
        }

        [TestMethod()]
        public void TestGetComments1()
        {
            int cnt = 10;
            long size;
            List<Comment> list;

            long userProfileId = createUserProfile(loginName);
            long commentId = createComment(userProfileId, txt);

            Comment comment = commentDao.Find(commentId);

            list = commentService.getComment(productId, 0, cnt);
            size = commentService.getNumberOfComment(productId);

            Assert.AreEqual(list.ToArray()[0].UserProfile.loginName, loginName);


            Assert.AreEqual(list.Count, size);
            Assert.AreEqual(size, 1);

            Assert.IsTrue(list.Contains(comment));
        }

        [TestMethod()]
        public void TestGetComments2() {
            int cnt = 2;
            long size;

            List<Comment> list;
            long userId1 = createUserProfile(loginName+"1");
            long userId2 = createUserProfile(loginName+"2");
            long commentId1 = createComment(userId1, txt);
            System.Threading.Thread.Sleep(1001);
            long commentId2 = createComment(userId2, txt);
            System.Threading.Thread.Sleep(1001);
            long commentId3 = createComment(userId1, txt);

            
            size = commentService.getNumberOfComment(productId);
            Assert.AreEqual(size, 3);

            list = commentService.getComment(productId, 0, cnt);
            Assert.AreEqual(list.Count, cnt);
            Assert.IsTrue (list.Contains(commentDao.Find(commentId3)));
            Assert.IsTrue (list.Contains(commentDao.Find(commentId2)));
            //Assert.IsFalse(list.Contains(commentDao.Find(commentId1)));

            list = commentService.getComment(productId, 2, cnt);
            Assert.AreEqual(list.Count, 1);
            Assert.IsTrue (list.Contains(commentDao.Find(commentId1)));
            //Assert.IsFalse(list.Contains(commentDao.Find(commentId3)));
            //Assert.IsFalse(list.Contains(commentDao.Find(commentId2)));
        }

        public void TestGetCommentByLabelId()
        {
            List<String> labelList = new List<String>();
            labelList.Add("etiqueta");


            long userId = createUserProfile();
            long userId2 = createUserProfile("federico");

            long commentId1 = createComment(userId, "comentario",labelList);
            long commentId2 = createComment(userId, "comentario2", labelList);
            long commentId3 = createComment(userId2, "lalalala", labelList);
            long commentId4 = createComment(userId2, "nananana", labelList);

            Label label = labelDao.findByName("etiqueta");

            List<Comment> list = commentService.getCommentsByLabelId(label.labelId, 0, 10);
            Assert.AreEqual(4, list.Count);

            foreach (Comment c in list)
            {
                Assert.IsTrue(c.Label.Contains(label));
            }

            List<Comment> list2 = commentService.getCommentsByLabelId(label.labelId, 0, 2);
            long number = commentService.getNumberOfCommentsByLabelId(label.labelId);

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(4, number);
        }

        #endregion

        #region Test for remove comment
        [TestMethod()]
        public void removeCommentTest()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);

            commentService.removeComment(userProfileId, commentId);

            try
            {
                commentDao.Find(commentId);
                Assert.IsTrue(false);
            }
            catch (InstanceNotFoundException)
            {
                Assert.IsTrue(true);
            }
        }
        [TestMethod()]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void removeNonExistentCommentTest()
        {
            commentService.removeComment(1, NON_EXISTENT_COMMENT_ID);
        }

        [TestMethod()]
        [ExpectedException(typeof(UnauthorizedException))]
        public void removeUnauthorizedException()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);

            commentService.removeComment(NON_EXISTENT_USER_ID, commentId);
        }

        #endregion

        #region test for labels

        [TestMethod()]
        public void addNullLabelTest()
        {
            List<string> l = new List<string>();
            l.Add(null);
            l.Add(" \t");
            l.Add("\t");
            l.Add("  ");
            l.Add("");

            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt, l);

            Comment comment = commentDao.Find(commentId);
            Assert.AreEqual(comment.Label.Count, 0);
        }

        [TestMethod()]
        public void addLabelTest1()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt, labels);

            Comment comment = commentDao.Find(commentId);
            Assert.AreEqual(labels.Count, comment.Label.Count);
            Assert.IsTrue(checkMatchLabels(labels, comment.Label));

            foreach (Label l in comment.Label)
                Assert.AreEqual(l.cnt, 1);
        }

        [TestMethod()]
        public void addLabelTest2()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);
            commentService.addLabels(commentId, labels, userProfileId);

            Comment comment = commentDao.Find(commentId);
            Assert.AreEqual(labels.Count, comment.Label.Count);
            Assert.IsTrue(checkMatchLabels(labels, comment.Label));

            foreach (Label l in comment.Label)
                Assert.AreEqual(l.cnt, 1);
        }

        [TestMethod()]
        public void addLabelTest3()
        {// Probar a anadir etiquetas vacias sobre el metodo addLabels
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt);
            List<string> emptyLabels = new List<string>();
            emptyLabels.Add(null);
            emptyLabels.Add("");
            emptyLabels.Add(" ");
            emptyLabels.Add("\t ");
            commentService.addLabels(commentId, emptyLabels, userProfileId);

            Comment comment = commentDao.Find(commentId);
            Assert.AreEqual(comment.Label.Count, 0);

            List<Label> cLabels = commentService.getLabelsByCommentId(commentId);
            Assert.AreEqual(cLabels.Count, 0);
        }


        [TestMethod()]
        public void removeLabelTest1()
        {
            long userProfileId = createUserProfile();
            long commentId = createComment(userProfileId, txt, labels);

            Comment comment = commentDao.Find(commentId);
            Assert.AreEqual(labels.Count, comment.Label.Count);
            Assert.IsTrue(checkMatchLabels(labels, comment.Label));

            foreach (Label l in comment.Label)
                Assert.AreEqual(l.cnt, 1);

            string str = labels.ToArray()[0];
            labels.Remove(str);

            List<string> list = new List<string>();
            list.Add(str);

            commentService.removeLabels(commentId, list, userProfileId);

            comment = commentDao.Find(commentId);
            Assert.AreEqual(labels.Count, comment.Label.Count);
            Assert.IsTrue(checkMatchLabels(labels, comment.Label));


            long cnt = labelDao.findByName(StrNormalize.strNormalize(str)).cnt;
            Assert.AreEqual(cnt, 0);

        }

        // Eliminar una etiqueta de un comentario (cuando tiene que quedar en otros)
        // Se prueba mirando si esa etiqueta solo esta en un comentario
        [TestMethod()]
        public void removeLabelTest2()
        {
            long userProfileId1 = createUserProfile();
            long commentId1 = createComment(userProfileId1, txt, labels);

            Comment comment = commentDao.Find(commentId1);
            Assert.AreEqual(labels.Count, comment.Label.Count);
            Assert.IsTrue(checkMatchLabels(labels, comment.Label));

            long userProfileId2 = createUserProfile("c" + loginName);
            long commentId2 = createComment(userProfileId2, txt, labels);

            comment = commentDao.Find(commentId2);
            Assert.AreEqual(labels.Count, comment.Label.Count);
            Assert.IsTrue(checkMatchLabels(labels, comment.Label));

            foreach (Label l in comment.Label)
                Assert.AreEqual(l.cnt, 2);

            string str = labels.ToArray()[0];
            labels.Remove(str);

            List<string> list = new List<string>();
            list.Add(str);

            commentService.removeLabels(commentId1, list, userProfileId1);

            comment = commentDao.Find(commentId1);
            Assert.AreEqual(labels.Count, comment.Label.Count);
            Assert.IsTrue(checkMatchLabels(labels, comment.Label));

            long cnt = labelDao.findByName(StrNormalize.strNormalize(str)).cnt;
            Assert.AreEqual(cnt, 1);
        }

        [TestMethod()]
        public void removeCommentCountLabelTest()
        {
            long userProfileId1 = createUserProfile();
            long commentId1 = createComment(userProfileId1, txt, labels);

            long userProfileId2 = createUserProfile("c" + loginName);
            long commentId2 = createComment(userProfileId2, txt, labels);

            commentService.removeComment(userProfileId1, commentId1);

            Comment comment = commentDao.Find(commentId2);

            foreach (Label l in comment.Label)
                Assert.AreEqual(l.cnt, 1);
        }

        [TestMethod()]
        public void TestGetLabelsByCommentid()
        {
            List<String> labelList = new List<String>();

            labelList.Add("etiqueta1");
            labelList.Add("etiqueta2");
            labelList.Add("etiqueta3");
            labelList.Add("etiqueta4");
            labelList.Add("etiqueta5");
            labelList.Add("etiqueta6");

            long userId = createUserProfile();
            long commentId = createComment(userId, "Comentario", labelList);

            List<Label> labels = commentService.getLabelsByCommentId(commentId);

            Assert.AreEqual(6,labels.Count);
            int i = 1;
            foreach (Label l in labels)
            {
                Assert.AreEqual("etiqueta" + i,l.name);
                i++;
            }
        }

        #endregion
    }

}