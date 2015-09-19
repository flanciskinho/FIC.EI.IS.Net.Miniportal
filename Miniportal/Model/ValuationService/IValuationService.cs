using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

using Es.Udc.DotNet.MiniPortal.Model.ValuationDao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;

namespace Es.Udc.DotNet.MiniPortal.Model.ValuationService
{
    public interface IValuationService
    {
        [Dependency]
        IValuationDao valuationDao { set; }

        /// <exception cref="DuplicateIntaceException" />
        /// <exception cref="InstanceNotFoundException" />
        [Transactional]
        long addValuation(long userId, string sellerId, long productId, long score,string txt);
        
        [Transactional]
        List<Valuation> listAllBySeller(string seller, int start, int cnt);

        [Transactional]
        Dictionary<string, AverageAndNumberOfValuations> getAverageAndNumberOfValuations(List<string> sellers);

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        AverageAndNumberOfValuations getAverageAndNumberOfValuations(string seller);

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        Valuation getByUserIdAndProductId(long userId, long productId);

    }
}
