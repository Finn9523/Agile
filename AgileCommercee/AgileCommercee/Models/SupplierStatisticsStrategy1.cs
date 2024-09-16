﻿using AgileCommercee.Data;

namespace AgileCommercee.Models
{
    public class SupplierStatisticsStrategy1 : IStatisticsStrategy1
    {
        public IEnumerable<object> GetStatistics(AgileStoreContext context)
        {
            return context.HangHoas
                .GroupBy(p => p.MaNccNavigation.TenCongTy)
                .Select(g => new SupplierStatistic
                {
                    MaNccNavigation = g.Key,
                    NumOfProduct = g.Count()
                })
                .ToList();
        }
    }
}
