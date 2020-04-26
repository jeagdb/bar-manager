using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarManagement.DataAccess.EfModels;

namespace BarManagement.DataAccess
{
    public class StocksRepository : Repository<EfModels.Stocks, Models.Stocks>, Interfaces.IStocksRepository
    {
        public StocksRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
        public List<Models.Stocks> GetStocks()
        {
            var result = _context.Stocks.ToList();
            return _mapper.Map<List<Models.Stocks>>(result);
        }
    }
}
