using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

using Es.Udc.DotNet.MiniPortal.Model.ValuationDao;
using Es.Udc.DotNet.MiniPortal.Model.UserProfileDao;
using Es.Udc.DotNet.MiniPortal.Model.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.MiniPortal.Model.ValuationService
{
    class ValuationService : IValuationService
    {
        [Dependency]
        public IValuationDao valuationDao { private get; set; }
        [Dependency]
        public IUserProfileDao userProfileDao { private get; set; }


        public Dictionary<string, AverageAndNumberOfValuations> getAverageAndNumberOfValuations(List<string> sellers)
        {
            return valuationDao.getAverageAndNumberOfValuations(sellers);
        }

        /// <exception cref="DuplicateIntaceException" />
        /// <exception cref="InstanceNotFoundException" />
        public long addValuation(long userId, string sellerId, long productId, long score, string txt) {
            // Comprobamos si existe el usuario
            userProfileDao.Find(userId);

            // Comprobamos si existe una valoracion para ese producto
            try
            {
                if (valuationDao.getByUserIdAndProductId(userId, productId) != null)
                    throw new DuplicateInstanceException(new long[2] { userId, productId }, typeof(Valuation).FullName);
            }
            catch (InstanceNotFoundException) { }

           Valuation valuation = Valuation.CreateValuation(0,score,DateTime.Now,sellerId,userId,productId);
           valuation.txt = txt;

           valuationDao.Create(valuation);

           return valuation.valuationId;
        }

        
        public List<Valuation> listAllBySeller(string seller, int start, int cnt)
        {
            return valuationDao.getValuationBySeller(seller, start, cnt);
        }

        /*
        /// <exception cref="InstanceNotFoundException"/>
        public AverageAndNumberOfValuations getAverageAndNumberOfValuations(string seller)
        {
            AverageAndNumberOfValuations anv;
            anv.average = valuationDao.getAverageOfValuations(seller);
            anv.numberOfValuations = valuationDao.getNumberOfValuations(seller);

            return anv;
        }
        */

        /// <exception cref="InstanceNotFoundException"/>
        public AverageAndNumberOfValuations getAverageAndNumberOfValuations(string seller)
        {
            List<string> list = new List<string>();
            list.Add(seller);

            Dictionary<string, AverageAndNumberOfValuations> dic = getAverageAndNumberOfValuations(list);

            try
            {
                return dic[seller];
            }
            catch (KeyNotFoundException)
            {
                throw new InstanceNotFoundException(seller, typeof(Valuation).FullName);
            }
        }

        /// <exception cref="InstanceNotFoundException"/>
        public Valuation getByUserIdAndProductId(long userId, long productId)
        {
            return valuationDao.getByUserIdAndProductId(userId, productId);
        }

    }
}
