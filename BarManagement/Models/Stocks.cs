using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.Models
{
    public class Stocks : IObjectWithId
    {
        public long Id { get; set; }
        public long DrinkId { get; set; }
        public long? Quantity { get; set; }
        public double? Price { get; set; }

        public virtual Drinks Drink { get; set; }
    }
}
