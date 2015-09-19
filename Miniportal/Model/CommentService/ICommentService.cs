using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

using Es.Udc.DotNet.MiniPortal.Model.CommentDao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;

namespace Es.Udc.DotNet.MiniPortal.Model.CommentService
{
    public interface ICommentService
    {
        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="EmptyStringException"/>
        [Transactional]
        long addComment(long userId, long productId, string txt, List<string> labelNames);

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        void addLabels(long commentId, List<string> labelNames, long userId);

        /// <exception cref="InstanceNotFoundException"/
        [Transactional]
        List<Label> getLabelsByCommentId(long commentId);

        [Transactional]
        List<Label> getLabels(int start, int cnt);

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        List<Comment> getCommentsByLabelId(long labelId, int start, int cnt);

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        Comment getCommentById(long commentId);

        [Transactional]
        long getNumberOfCommentsByLabelId(long labelId);

        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="UnauthorizedException" />
        [Transactional]
        void removeLabels(long commentId, List<string> labelNames, long userProfileId);

        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="UnauthorizedException" />
        [Transactional]
        void removeComment(long userProfileId, long commentId);

        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="EmptyStringException"/>
        /// <exception cref="UnauthorizedException" />
        [Transactional]
        void modifyComment(long commentId, string txt, long userId, List<String> tags);

        [Transactional]
        List<Comment> getComment(long productId, int start, int cnt);

        [Transactional]
        long getNumberOfComment(long productId);
    }
}
