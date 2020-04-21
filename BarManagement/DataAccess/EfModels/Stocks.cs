using System;
using System.Collections.Generic;

namespace BarManagement.DataAccess.EfModels
{
    public partial class Stocks
    {
        public long Id { get; set; }
        public long DrinkId { get; set; }
        public long? Quantity { get; set; }
        public double? Price { get; set; }

        public virtual Drinks Drink { get; set; }
    }
}
