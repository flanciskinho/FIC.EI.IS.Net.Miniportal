using System;
using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.LabelDao;
using System.Linq;
using System.Data.Metadata.Edm;
using System.Text;
using Microsoft.Practices.Unity;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace Es.Udc.DotNet.MiniPortal.Model.CommentDao
{
    public class CommentDaoEntityFramework : 
        GenericDaoEntityFramework<Comment, Int64>, ICommentDao
    {

        [Dependency]
        public ILabelDao labelDao { private get; set; }

        public CommentDaoEntityFramework() { }



        /// <exception cref="InstanceNotFoundException"/>
        public Comment getCommentAndUserProfile(long commentId)
        {
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            var result =
                (from c in comments.Include("UserProfile").Include("Label")
                 where c.commentId == commentId
                 select c);

            Comment tmp = result.FirstOrDefault();
            if (tmp == null)
            {
                throw new InstanceNotFoundException(commentId, typeof(Comment).FullName);
            }

            return tmp;
        }

        public List<Comment> getCommentByDate(long prodcutId,int start, int cnt) {
            List<Comment> list = null;
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            var result =
                (from c in comments.Include("UserProfile").Include("Label")
                 where c.productId == prodcutId
                 orderby c.addDate descending
                 select c).Skip(start).Take(cnt);

           return list = result.ToList();
        }

        public long getNumberOfComment(long productId) {
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            return (from c in comments
                   where c.productId == productId
                   select c).Count();
        }

        public List<Label> getLabelsByCommentId(long commentId)
        {
            ObjectSet<Comment> comments = Context.CreateObjectSet<Comment>();

            var result = (from c in comments
                              where c.commentId == commentId
                              select c.Label).FirstOrDefault();
            return result.ToList();
        }
    }
}
