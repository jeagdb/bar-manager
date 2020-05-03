using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarManagement.DataAccess.EfModels;
using BarManagement.DataAccess.Interfaces;

namespace BarManagement.DataAccess
{
    public class TransactionsRepository : Repository<Transactions, Models.Transactions>, ITransactionsRepository
    {
        public TransactionsRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public List<Models.Transactions> GetTransactions()
        {
            var result = _context.Transactions.ToList();
            return _mapper.Map<List<Models.Transactions>>(result);
        }
    }
}
