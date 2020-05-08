using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BarManagement.DataAccess;
using BarManagement.DataAccess.Interfaces;
using barManagementTests.Fixtures;

using BM = BarManagement;
using AutoMapper;

namespace barManagementTests.DataAccess
{
    [Collection("testBarDB")]
    public class StocksRepository
    {
        barFixture _fixture;
        public StocksRepository(barFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void getStocks()
        {
            IStocksRepository repository = GetStocksRepository();
            List<BM.Models.Stocks> stocks = repository.GetStocks();

            Assert.NotNull(stocks);
            Assert.Equal(3, stocks.Count);
            Assert.Equal("Coca Cola", stocks[0].Drink.Name);

        }
        private IStocksRepository GetStocksRepository()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfiles());
            });
            var mapper = config.CreateMapper();
            return new BM.DataAccess.StocksRepository(_fixture.getContext(), mapper);
        }
    }
}
