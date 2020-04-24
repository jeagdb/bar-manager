using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarManagement.DataAccess.EfModels;

namespace BarManagement.DataAccess
{
    public class TransactionsRepository : Repository<Transactions, Models.Transactions>
    {
        public TransactionsRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
