using AutoMapper;
using BarManagement.DataAccess;
using BarManagement.DataAccess.Interfaces;
using barManagementTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

using BM = BarManagement;

namespace barManagementTests.DataAccess
{ 
    [Collection("testBarDB")]
    public class CocktailsRepository
    {
        barFixture _fixture;

        public CocktailsRepository(barFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void getCocktails()
        {
            ICocktailsRepository repository = GetCocktailsRepository();
            List<BM.Models.Cocktails> cocktails = repository.GetCocktails();

            Assert.NotNull(cocktails);
            Assert.Equal(2, cocktails.Count);
            Assert.Equal("Pina Colada", cocktails[0].Name);

        }

        private ICocktailsRepository GetCocktailsRepository()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfiles());
            });
            var mapper = config.CreateMapper();
            return new BM.DataAccess.CocktailsRepository(_fixture.getContext(), mapper);
        }
    }
}
