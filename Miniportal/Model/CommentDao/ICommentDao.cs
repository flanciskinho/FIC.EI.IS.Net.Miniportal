using System;
using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Model.CommentDao
{
    public interface ICommentDao : IGenericDao<Comment, Int64>
    {
       List<Comment> getCommentByDate(long prodcutId, int start, int cnt);

       long getNumberOfComment(long productId);

       List<Label> getLabelsByCommentId(long commentId);

       /// <exception cref="InstanceNotFoundException"/>
       Comment getCommentAndUserProfile(long commentId);
    }
}
