using System;
using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Model.LabelDao
{
    public interface ILabelDao : IGenericDao<Label, Int64>
    {
        /// <exception cref="InstanceNotFoundException"/>
        Label findByName(string str);

        List<Label> getCommentLabel(int start, int cnt);

        long getNumberOfLabels();

        List<Comment> getCommentByLabelId(long labelId, int start, int cnt);

        long getNumberOfCommentByLabelId(long labelId);
    }
}
