using System;
using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.MiniPortal.Model.ValuationService;

namespace Es.Udc.DotNet.MiniPortal.Model.ValuationDao
{
    public interface IValuationDao : IGenericDao<Valuation, Int64>
    {

        Dictionary<string, AverageAndNumberOfValuations> getAverageAndNumberOfValuations(List<string> sellers);

        List<Valuation> getValuationBySeller(string seller, int startIndex, int count);

        /*
        long getNumberOfValuations(string seller);
        /// <exception cref="InstanceNotFoundException"/>
        double getAverageOfValuations(string seller);
        */

        /// <exception cref="InstanceNotFoundException"/>
        Valuation getByUserIdAndProductId(long userProfileId, long productId);
    }
}
