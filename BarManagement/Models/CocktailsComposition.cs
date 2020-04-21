using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.Models
{
    public class CocktailsComposition : IObjectWithId
    {
        public long CocktailId { get; set; }
        public long DrinkId { get; set; }
        public long Quantity { get; set; }

        public virtual Cocktails Cocktail { get; set; }
        public virtual Drinks Drink { get; set; }
    }
}
