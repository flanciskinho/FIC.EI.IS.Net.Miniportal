using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Es.Udc.DotNet.MiniPortal.Model.CommentDao;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.UserProfileDao;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.LabelDao;
using Es.Udc.DotNet.MiniPortal.Model.CommentService.Util;

namespace Es.Udc.DotNet.MiniPortal.Model.CommentService
{
    class CommentService : ICommentService
    {
        [Dependency]
        public IUserProfileDao userProfileDao { private get; set; }

        [Dependency]
        public ICommentDao commentDao { private get; set; }

        [Dependency]
        public ILabelDao labelDao { private get; set; }

        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="EmptyStringException"/>
        public long addComment(long userId, long productId, string txt, List<string> labelNames)
        {
            if (String.IsNullOrWhiteSpace(txt))
                throw new EmptyStringException();

            // Comprobamos si existe el usuario
            userProfileDao.Find(userId);

            Comment comment = Comment.CreateComment(0, txt, DateTime.Now, productId, userId);

            LabelComment(comment, labelNames, userId);

            //Guardamos el comentario en la bbdd
            commentDao.Create(comment);

            return comment.commentId;
        }

        /// <exception cref="InstanceNotFoundException"/>
        public void addLabels(long commentId, List<string> labelNames, long userId)
        {
            Comment comment = commentDao.Find(commentId);
            LabelComment(comment, labelNames,userId);
            commentDao.Update(comment);
        }

        /// <exception cref="InstanceNotFoundException"/>
        public List<Label> getLabelsByCommentId(long commentId)
        {
            return commentDao.getLabelsByCommentId(commentId);
        }

        public List<Label> getLabels(int start, int cnt){

            List<Label> labels = labelDao.getCommentLabel(start, cnt);

            return labels;
        }

        /// <exception cref="InstanceNotFoundException"/>
        public List<Comment> getCommentsByLabelId(long labelId, int start, int cnt)
        {
            return labelDao.getCommentByLabelId(labelId, start, cnt);
        }

        public long getNumberOfCommentsByLabelId(long labelId)
        {
            return labelDao.getNumberOfCommentByLabelId(labelId);
        }

        /// <exception cref="InstanceNotFoundException"/>
        public Comment getCommentById(long commentId)
        {
            return commentDao.getCommentAndUserProfile(commentId);
        }

        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="UnauthorizedException" />
        public void removeLabels(long commentId, List<string> labelNames, long userProfileId)
        {
            Comment comment = commentDao.Find(commentId);
            if (comment.userProfileId != userProfileId)
                throw new UnauthorizedException();

            foreach(string str in labelNames)
            {
                string labelName = StrNormalize.strNormalize(str);

                try
                {
                    Label label = labelDao.findByName(labelName);
                    label.cnt--;
                    comment.Label.Remove(label);
                }
                catch (InstanceNotFoundException) { }
            }

            commentDao.Update(comment);
        }

        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="UnauthorizedException" />
        public void removeComment(long userProfileId, long commentId)
        {
            Comment comment = commentDao.Find(commentId);
            if (comment.userProfileId != userProfileId)
                throw new UnauthorizedException();

            if (comment.Label != null)
            {
                foreach (Label label in comment.Label)
                    label.cnt--;
            }

            commentDao.Remove(commentId);
        }

        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="EmptyStringException"/>
        /// <exception cref="UnauthorizedException" />
        public void modifyComment(long commentId, string txt, long userId, List<String> tags)
        {
            if (String.IsNullOrWhiteSpace(txt))
                throw new EmptyStringException();

            Comment comment;
            Label label;
            comment = commentDao.Find(commentId);
            if (comment == null)
                throw new InstanceNotFoundException(commentId, typeof(Comment).FullName);
            if (userId != comment.userProfileId)
                throw new UnauthorizedException();

            if (tags != null)
            {
                foreach (Label l in comment.Label.ToList())
                {
                    comment.Label.Remove(l);
                    l.cnt--;
                }

                foreach (String s in tags)
                {
                    string labelName = StrNormalize.strNormalize(s);
                    try
                    {
                        label = labelDao.findByName(labelName);
                    }
                    catch (InstanceNotFoundException)
                    {
                        label = Label.CreateLabel(0, labelName, 0);
                    } comment.Label.Add(label);
                    label.cnt++;
                }
            }
            comment.txt = txt;
            comment.addDate = DateTime.Now;

            commentDao.Update(comment);

        }

        public List<Comment> getComment(long productId, int start, int cnt) 
        {
            return commentDao.getCommentByDate(productId, start, cnt);
        }

        public long getNumberOfComment(long productId)
        {
            return commentDao.getNumberOfComment(productId);
        }

        private void LabelComment(Comment comment, List<string> labelNames, long userId)
        {
            if (labelNames == null || labelNames.Count == 0)
                return;

            foreach (string str in labelNames)
            {
                if (String.IsNullOrWhiteSpace(str))
                    continue;

                Label label;
                // "normalizar" el nombre
                string labelName = StrNormalize.strNormalize(str);
                // existe el tag
                try
                {
                    label = labelDao.findByName(labelName);
                }
                catch (InstanceNotFoundException)
                {
                    label = Label.CreateLabel(0, labelName, 0);
                }

                if (!comment.Label.Contains(label))
                {
                    label.cnt++;
                    comment.Label.Add(label);
                }
            }
        }
    }
}
