using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

using Es.Udc.DotNet.MiniPortal.Model.FavouriteDao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;

namespace Es.Udc.DotNet.MiniPortal.Model.FavouriteService
{
    public interface IFavouriteService
    {
        [Dependency]
        IFavouriteDao favouriteDao { set; }

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        Favourite getByUserIdAndProductId(long userId, long productId);

        /// <exception cref="DuplicateIntaceException" />
        /// <exception cref="InstanceNotFoundException" />
        /// <exception cref="EmptyStringException" />
        [Transactional]
        long addFavourite(long userId, long productId, string name, string comment);

        [Transactional]
        List<Favourite> getFavouriteByUserId(long userId, int start, int cnt);

        [Transactional]
        long getNumberFavouriteByUserProfileId(long userProfileId);

        /// <exception cref="InstanceNotFoundException" />
        /// <exception cref="UnauthorizedException" />
        [Transactional]
        void removeFromList(long favId, long userId);
    }
}
