using AgileCommercee.Data;

namespace AgileCommercee.Models
{
    public class SupplierCategoryStatisticsStrategy1 : IStatisticsStrategy1
    {
        public IEnumerable<object> GetStatistics(AgileStoreContext context)
        {
            return context.HangHoas
                .GroupBy(p => new
                {
                    p.MaLoaiNavigation.TenLoai,
                    p.MaNccNavigation.TenCongTy
                })
                .Select(g => new SupplierCategoryStatistic
                {
                    MaLoaiNavigation = g.Key.TenLoai,
                    MaNccNavigation = g.Key.TenCongTy,
                    NumOfProduct = g.Count()
                })
                .ToList();
        }
    }
}
