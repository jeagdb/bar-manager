using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace barManagementTests.Models
{
    public class Transactions
    {
        [Fact]
        public void Constructor()
        {
            var transactionResult = new BarManagement.Models.Transactions();
            Assert.NotNull(transactionResult);
        }

        [Fact]
        public void InitializeWithCocktail()
        {
            var transactionResult = new BarManagement.Models.Transactions();
            transactionResult.Cocktail = new BarManagement.Models.Cocktails();
            Assert.NotNull(transactionResult);
            Assert.NotNull(transactionResult.Cocktail);
        }
    }
}
