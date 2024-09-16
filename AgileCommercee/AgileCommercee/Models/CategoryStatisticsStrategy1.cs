using AgileCommercee.Data;

namespace AgileCommercee.Models
{
    public class CategoryStatisticsStrategy1 : IStatisticsStrategy1
    {
        public IEnumerable<object> GetStatistics(AgileStoreContext context)
        {
            return context.HangHoas
                .GroupBy(p => p.MaLoaiNavigation.TenLoai)
                .Select(g => new CategoryStatistic
                {
                    MaLoaiNavigation = g.Key,
                    NumOfProduct = g.Count()
                })
                .ToList();
        }
    }
}
