using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using Microsoft.Practices.Unity;

namespace Es.Udc.DotNet.MiniPortal.Model.LabelDao
{
    public class LabelDaoEntityFramework :
        GenericDaoEntityFramework<Label, Int64>, ILabelDao
    {

        public LabelDaoEntityFramework() { }

        /// <exception cref="InstanceNotFoundException"/>
        public Label findByName(string str)
        {
            if (String.IsNullOrEmpty(str))
                throw new InstanceNotFoundException(str, typeof(Label).FullName);

            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            Label label =
                (from l in labels
                 where l.name == str
                 select l).FirstOrDefault();

            if (label == null)
                throw new InstanceNotFoundException(str, typeof(Label).FullName);

            return label;
        }

        public List<Label> getCommentLabel(int start, int cnt)
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            return (from l in labels
                    orderby l.cnt descending, l.name ascending
                    select l).Skip(start).Take(cnt).ToList();
        }

        public long getNumberOfLabels()
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            return (from l in labels
                    select l).Count();
        }

        public List<Comment> getCommentByLabelId(long labelId, int start, int cnt)
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            var result =
                (from l in labels
                 where l.labelId == labelId
                 select l.Comment).FirstOrDefault();

            return result.Skip(start).Take(cnt).ToList();
        }

        public long getNumberOfCommentByLabelId(long labelId)
        {
            ObjectSet<Label> labels = Context.CreateObjectSet<Label>();

            var result =
                (from l in labels
                 where l.labelId == labelId
                 select l.Comment).FirstOrDefault();

            return result.Count;

        }
    }
}
