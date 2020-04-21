using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.Models
{
    public class Cocktails : IObjectWithId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double PriceToSell { get; set; }
        public double Cost { get; set; }
    }
}
