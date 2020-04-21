using System;
using System.Collections.Generic;

namespace BarManagement.DataAccess.EfModels
{
    public partial class Drinks
    {
        public Drinks()
        {
            Stocks = new HashSet<Stocks>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }

        public virtual ICollection<Stocks> Stocks { get; set; }
    }
}
