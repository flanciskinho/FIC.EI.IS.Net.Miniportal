using System;
using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Model.FavouriteDao
{
    public interface IFavouriteDao : IGenericDao<Favourite, Int64>
    {
        /// <exception cref="InstanceNotFoundException"/>
        Favourite getByUserIdAndProductId(long userProfileId, long productId);

        List<Favourite> getByUserProfileId(long userProfileId, int start, int cnt);

        long getNumberFavouriteByUserProfileId(long userProfileId);
    }
}
