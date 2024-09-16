using AgileCommercee.Data;
using System.Collections.Generic;

namespace AgileCommercee.Models
{
    public interface IStatisticsStrategy1
    {
        IEnumerable<object> GetStatistics(AgileStoreContext context);
    }
}
