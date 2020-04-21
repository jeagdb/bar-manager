using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.Models
{
    public class Transactions : IObjectWithId
    {
        public long Id { get; set; }
        public DateTime SellDate { get; set; }
        public double Value { get; set; }
    }
}
