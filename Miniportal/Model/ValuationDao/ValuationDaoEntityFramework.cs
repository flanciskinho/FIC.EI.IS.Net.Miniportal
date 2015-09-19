using System;
using System.Data.Objects;
using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Linq;
using System.Text;
using Es.Udc.DotNet.MiniPortal.Model.ValuationService;

namespace Es.Udc.DotNet.MiniPortal.Model.ValuationDao
{
    public class ValuationDaoEntityFramework :
        GenericDaoEntityFramework<Valuation, Int64>, IValuationDao
    {
        public ValuationDaoEntityFramework() { }



        public Dictionary<string, AverageAndNumberOfValuations> getAverageAndNumberOfValuations(List<string> sellers)
        {
            ObjectSet<Valuation> valuations =
                Context.CreateObjectSet<Valuation>();

            var ret = (from v in valuations
                       where sellers.Contains(v.sellerId)
                       group v by v.sellerId into grp
                       select new 
                       {
                           seller = grp.Key,
                           average = grp.Average(v => v.score),
                           numberOfValuations = grp.Count(),
                           
                       });

            Dictionary<string, AverageAndNumberOfValuations> dic = new Dictionary<string, AverageAndNumberOfValuations>();
            foreach (var tmp in ret)
            {
                AverageAndNumberOfValuations anv;
                anv.average = tmp.average;
                anv.numberOfValuations = tmp.numberOfValuations;
                
                dic.Add(tmp.seller, anv);
            }

            return dic;
        }

        public List<Valuation> getValuationBySeller(string seller, int startIndex, int count) {
            ObjectSet<Valuation> valuations = 
                Context.CreateObjectSet<Valuation>();

            return (from v in valuations.Include("UserProfile")
                 where v.sellerId == seller
                 orderby v.addDate descending
                 select v).Skip(startIndex).Take(count).ToList();
        }

        /*
        public long getNumberOfValuations(string seller)
        {
            ObjectSet<Valuation> valuations =
                Context.CreateObjectSet<Valuation>();

            return (from v in valuations
                    where v.sellerId == seller
                    select v).Count();
        }

        /// <exception cref="InstanceNotFoundException"/>
        public double getAverageOfValuations(string seller)
        {
            ObjectSet<Valuation> valuations =
                Context.CreateObjectSet<Valuation>();

            var query = (from v in valuations
                    where v.sellerId == seller
                    select v.score);

            if (query.Count() == 0)
            {
                throw new InstanceNotFoundException(seller, typeof(Valuation).FullName);
            }

            return query.Average();

        }
        */

        /// <exception cref="InstanceNotFoundException"/>
        public Valuation getByUserIdAndProductId(long userProfileId, long productId)
        {
            ObjectSet<Valuation> valuation = Context.CreateObjectSet<Valuation>();

            Valuation val = null;
            var r = (from v in valuation
                     where v.userProfileId == userProfileId
                        && v.productId == productId
                     select v);

            val = r.FirstOrDefault();

            if (val == null)
            {
                throw new InstanceNotFoundException(
                    new long[2] { userProfileId, productId },
                    typeof(Valuation).FullName);
            }

            return val;

        }
    }
}
