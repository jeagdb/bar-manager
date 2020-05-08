using AutoMapper;
using BarManagement.DataAccess;
using BarManagement.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using BM = BarManagement;

namespace barManagementTests.DataAccess
{
    [Collection("testBarDB")]

    public class CocktailsCompositionRepository
    {
        Fixtures.barFixture _fixture;
        public CocktailsCompositionRepository(Fixtures.barFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void getCompositionByCocktailId()
        {
            ICocktailsCompositionRepository repository = GetCocktailsCompositionRepository();
            List<BM.Models.CocktailsComposition> compositions = repository.getCompositionByCocktailId(1);

            Assert.NotNull(compositions);
            Assert.Equal(2, compositions.Count);
            Assert.Equal(2, compositions[0].DrinkId);
            Assert.Equal(3, compositions[1].DrinkId);

        }

        private ICocktailsCompositionRepository GetCocktailsCompositionRepository()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfiles());
            });
            var mapper = config.CreateMapper();
            return new BM.DataAccess.CocktailsCompositionRepository(_fixture.getContext(), mapper);
        }
    }
}
