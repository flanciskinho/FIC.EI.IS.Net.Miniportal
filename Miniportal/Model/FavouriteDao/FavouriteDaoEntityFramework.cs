using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Metadata.Edm;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Data.Objects;

namespace Es.Udc.DotNet.MiniPortal.Model.FavouriteDao
{
    class FavouriteDaoEntityFramework :
        GenericDaoEntityFramework<Favourite, Int64>, IFavouriteDao
    {
        public FavouriteDaoEntityFramework() { }

        
        /// <exception cref="InstanceNotFoundException"/>
        public Favourite getByUserIdAndProductId(long userProfileId, long productId)
        {
            ObjectSet<Favourite> favourites = Context.CreateObjectSet<Favourite>();

            Favourite fav = null;
            var r = (from f in favourites
                     where f.userProfileId == userProfileId
                        && f.productId == productId
                     select f);

            fav = r.FirstOrDefault();

            if (fav == null)
            {
                throw new InstanceNotFoundException(
                    new long[2] { userProfileId, productId },
                    typeof(Favourite).FullName);
            }

            return fav;

        }

        public List<Favourite> getByUserProfileId(long userProfileId, int start, int cnt)
        {

            ObjectSet<Favourite> favourites = Context.CreateObjectSet<Favourite>();

            return
                (from f in favourites
                 where f.userProfileId == userProfileId
                 orderby f.addDate descending
                 select f).Skip(start).Take(cnt).ToList();

        }

        public long getNumberFavouriteByUserProfileId(long userProfileId) {
            ObjectSet<Favourite> favourites = Context.CreateObjectSet<Favourite>();

            return (from f in favourites
                    where f.userProfileId == userProfileId
                    select f).Count();
        }
    }
}
