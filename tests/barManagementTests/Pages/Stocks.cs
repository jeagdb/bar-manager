using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


using BM = BarManagement;

namespace barManagementTests.Pages
{
    public class Stocks
    {
        [Fact]
        public void calculatePricePerCl()
        {
            BM.Pages.StocksModel stocks = new BM.Pages.StocksModel(null, null, null, null, null);
            var res = stocks.calculatePricePerCl(2, 30);
            Assert.Equal(0.07, res);
        }

        [Fact]
        public void calculateTotalQuantity()
        {
            BM.Pages.StocksModel stocks = new BM.Pages.StocksModel(null, null,  null, null, null);
            var res = stocks.calculateTotalQuantity(2, 50);
            Assert.Equal(100, res);
        }
    }
}
