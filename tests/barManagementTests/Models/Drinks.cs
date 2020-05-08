using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace barManagementTests.Models
{
    public class Drinks
    {
        [Fact]
        public void Constructor()
        {
            var drinkResult = new BarManagement.Models.Drinks();
            Assert.NotNull(drinkResult);
        }

        [Fact]
        public void InitializeWithStocks()
        {
            var drinkResult = new BarManagement.Models.Drinks();
            drinkResult.Stocks = new List<BarManagement.Models.Stocks>();
            Assert.NotNull(drinkResult);
            Assert.Equal(0, drinkResult.Stocks.Count);
        }

    }
}
