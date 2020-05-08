using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using BM = BarManagement;
using BarManagement.DataAccess;
using BarManagement.DataAccess.Interfaces;

namespace barManagementTests.DataAccess
{
    [Collection("testBarDB")]
    public class DrinksRepository
    {
        Fixtures.barFixture _fixture;
        public DrinksRepository(Fixtures.barFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void GetDrinkById()
        {
            IDrinksRepository drinksRepository = GetDrinksRepository();
            BarManagement.Models.Drinks drink = drinksRepository.GetDrinkById(1);

            Assert.NotNull(drink);
            Assert.Equal("Coca Cola", drink.Name);

        }

        [Fact]
        public void GetDrinks()
        {
            IDrinksRepository drinksRepository = GetDrinksRepository();
            List<BarManagement.Models.Drinks> drinks = drinksRepository.GetDrinks();

            Assert.NotNull(drinks);
            Assert.Equal(3, drinks.Count);
        }

        private IDrinksRepository GetDrinksRepository()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfiles());
            });
            var mapper = config.CreateMapper();
            return new BM.DataAccess.DrinksRepository(_fixture.getContext(), mapper);
        }
    }
}
