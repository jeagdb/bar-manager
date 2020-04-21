using System;
using System.Collections.Generic;

namespace BarManagement.DataAccess.EfModels
{
    public partial class CocktailsComposition
    {
        public long CocktailId { get; set; }
        public long DrinkId { get; set; }
        public long Quantity { get; set; }

        public virtual Cocktails Cocktail { get; set; }
        public virtual Drinks Drink { get; set; }
    }
}
