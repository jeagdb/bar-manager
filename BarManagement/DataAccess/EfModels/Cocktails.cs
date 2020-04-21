using System;
using System.Collections.Generic;

namespace BarManagement.DataAccess.EfModels
{
    public partial class Cocktails
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double PriceToSell { get; set; }
        public double Cost { get; set; }
    }
}
