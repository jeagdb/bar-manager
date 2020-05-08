using BarManagement.DataAccess.EfModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace barManagementTests.Fixtures
{
    public class barFixture : IDisposable
    {
        public barDBContext barDBContext { get; set; }

        public barFixture()
        {
            DbContextOptions<barDBContext> options;
            var builder = new DbContextOptionsBuilder<barDBContext>();
            builder.UseInMemoryDatabase("testBarDB");
            options = builder.Options;
            barDBContext = new barDBContext(options);
            setBarFixture();
        }

        public void Dispose()
        {
            barDBContext.Dispose();
        }

        private void setBarFixture()
        {
            barDBContext.Database.EnsureDeleted();
            barDBContext.Database.EnsureCreated();

            barDBContext.Cocktails.Add(new Cocktails { Id = 1, Name = "Pina Colada", Cost = 8, PriceToSell = 8, CocktailCategory = "Cocktail" });
            barDBContext.Cocktails.Add(new Cocktails { Id = 2, Name = "Coca Cola", Cost = 8, PriceToSell = 5, CocktailCategory = "Soft" });

            barDBContext.Drinks.Add(new Drinks { Id = 1, Name = "Coca Cola", Brand = "Coca", Category = "Soft" });
            barDBContext.Drinks.Add(new Drinks { Id = 2, Name = "Ananas", Brand = "Joker", Category = "Soft" });
            barDBContext.Drinks.Add(new Drinks { Id = 3, Name = "Vodka", Brand = "Ivanov", Category = "Alcool" });

            barDBContext.Stocks.Add(new Stocks { Id= 1, DrinkId = 1, Price = 0.2, Quantity = 10});
            barDBContext.Stocks.Add(new Stocks { Id = 2, DrinkId = 2, Price = 0.4, Quantity = 10});
            barDBContext.Stocks.Add(new Stocks { Id = 3, DrinkId = 3, Price = 0.5, Quantity = 10});

            barDBContext.CocktailsComposition.Add(new CocktailsComposition { Id = 1, CocktailId = 1, DrinkId = 2, Quantity = 25});
            barDBContext.CocktailsComposition.Add(new CocktailsComposition { Id = 2, CocktailId = 1, DrinkId = 3, Quantity = 5});
            barDBContext.CocktailsComposition.Add(new CocktailsComposition { Id = 3, CocktailId = 2, DrinkId = 1, Quantity = 33});

            barDBContext.Transactions.Add(new Transactions { Id = 1, CocktailId = 1, SellDate = DateTime.Now, Value = 8});

            barDBContext.SaveChanges();
        }

        public barDBContext getContext()
        {
            return barDBContext;
        }
    }
}
