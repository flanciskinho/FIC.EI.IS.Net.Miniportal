using System;
using System.Collections.Generic;
using Es.Udc.DotNet.MiniPortal.Model.FavouriteDao;
using Microsoft.Practices.Unity;
using Es.Udc.DotNet.MiniPortal.Model.UserProfileDao;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Model.FavouriteService
{
    public class FavouriteService : IFavouriteService
    {
        [Dependency]
        public IFavouriteDao favouriteDao { private get; set; }
        [Dependency]
        public IUserProfileDao userProfileDao { private get; set; }

        /// <exception cref="InstanceNotFoundException"/>
        public Favourite getByUserIdAndProductId(long userId, long productId)
        {
            return favouriteDao.getByUserIdAndProductId(userId, productId);
        }

        /// <exception cref="DuplicateIntaceException" />
        /// <exception cref="InstanceNotFoundException" />
        /// <exception cref="EmptyStringException" />
        public long addFavourite(long userId, long productId, string name, string comment)
        {
            // Comprobamos si existe el usuario
            userProfileDao.Find(userId);

            // Comprobamos si existe ya un favorito para ese producto
            try {
                if (favouriteDao.getByUserIdAndProductId(userId, productId) != null)
                {
                    throw new DuplicateInstanceException(new long[2]{userId, productId}, typeof(Favourite).FullName);
                }
            } catch (InstanceNotFoundException) {

            }

            if (String.IsNullOrWhiteSpace(name))
                throw new EmptyStringException();

            Favourite favourite = Favourite.CreateFavourite(0,name,DateTime.Now,productId,userId);
            favourite.comment = comment;

            favouriteDao.Create(favourite);

            return favourite.favouriteId;
        }

        public List<Favourite> getFavouriteByUserId(long userId,int start, int cnt)
        {
            return favouriteDao.getByUserProfileId(userId, start, cnt);
        }

        public long getNumberFavouriteByUserProfileId(long userProfileId)
        {
            return favouriteDao.getNumberFavouriteByUserProfileId(userProfileId);
        }

        /// <exception cref="InstanceNotFoundException" />
        /// <exception cref="UnauthorizedException" />
        public void removeFromList(long favId, long userId)
        {
            Favourite favourite = favouriteDao.Find(favId);

            if (userId != favourite.userProfileId)
                throw new UnauthorizedException();

            favouriteDao.Remove(favId);
        }
    }
}
