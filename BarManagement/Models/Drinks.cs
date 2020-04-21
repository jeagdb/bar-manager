using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.Models
{
    public class Drinks : IObjectWithId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }

        public virtual ICollection<Stocks> Stocks { get; set; }
    }
}
