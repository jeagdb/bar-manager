using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace barManagementTests.Models
{
    public class CocktailsComposition
    {
        [Fact]
        public void Constructor()
        {
            var cocktailCompositionResult = new BarManagement.Models.CocktailsComposition();
            Assert.NotNull(cocktailCompositionResult);
        }

        [Fact]
        public void InitializeWithCocktail()
        {
            var cocktailCompositionResult = new BarManagement.Models.CocktailsComposition();
            var cocktailResult = new BarManagement.Models.Cocktails();
            cocktailCompositionResult.Cocktail = cocktailResult;
            Assert.NotNull(cocktailCompositionResult);
            Assert.NotNull(cocktailCompositionResult.Cocktail);
        }

        [Fact]
        public void InitializeWithDrink()
        {
            var cocktailCompositionResult = new BarManagement.Models.CocktailsComposition();
            var drinkResult = new BarManagement.Models.Drinks();
            cocktailCompositionResult.Drink = drinkResult;
            Assert.NotNull(cocktailCompositionResult);
            Assert.NotNull(cocktailCompositionResult.Drink);
        }
    }
}
