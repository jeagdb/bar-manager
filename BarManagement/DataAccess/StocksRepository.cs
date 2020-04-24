using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarManagement.DataAccess.EfModels;

namespace BarManagement.DataAccess
{
    public class StocksRepository : Repository<Stocks, Models.Stocks>
    {
        public StocksRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
