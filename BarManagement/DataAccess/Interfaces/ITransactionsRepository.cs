using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.DataAccess.Interfaces
{
    public interface ITransactionsRepository : IRepository<EfModels.Transactions, Models.Transactions>
    {
        List<Models.Transactions> GetTransactions();
    }
}
