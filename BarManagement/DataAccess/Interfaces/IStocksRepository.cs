using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.DataAccess.Interfaces
{
    public interface IStocksRepository : IRepository<EfModels.Stocks, Models.Stocks>
    {
        List<Models.Stocks> GetStocks();
    }
}
