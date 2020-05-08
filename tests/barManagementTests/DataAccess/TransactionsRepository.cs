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
    public class TransactionsRepository
    {
        Fixtures.barFixture _fixture;
        public TransactionsRepository(Fixtures.barFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void GetTransactions()
        {
            ITransactionsRepository transactionsRepository = GetTransactionsRepository();
            List<BM.Models.Transactions> transactions = transactionsRepository.GetTransactions();

            Assert.NotNull(transactions);
            Assert.Single(transactions);
            Assert.Equal(8, transactions[0].Value);
        }

        [Fact]
        public void GetTransactionsByCocktail()
        {
            ITransactionsRepository transactionsRepository = GetTransactionsRepository();
            List<BM.Models.Transactions> transactions = transactionsRepository.GetTransactionsByCocktail(1);
            Assert.NotNull(transactions);
            Assert.Single(transactions);
            Assert.Equal(8, transactions[0].Value);
        }
        private ITransactionsRepository GetTransactionsRepository()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfiles());
            });
            var mapper = config.CreateMapper();
            return new BM.DataAccess.TransactionsRepository(_fixture.getContext(), mapper);
        }
    }
}
