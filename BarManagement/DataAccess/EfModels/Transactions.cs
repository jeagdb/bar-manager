using System;
using System.Collections.Generic;

namespace BarManagement.DataAccess.EfModels
{
    public partial class Transactions
    {
        public long Id { get; set; }
        public DateTime SellDate { get; set; }
        public double Value { get; set; }
    }
}
