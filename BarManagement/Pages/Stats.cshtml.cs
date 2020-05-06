using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BarManagement.Pages
{
    public class StatsModel : PageModel
    {
        private readonly DataAccess.Interfaces.ICocktailsRepository _cocktailsRepository;
        private readonly DataAccess.Interfaces.ITransactionsRepository _transactionsRepository;

        public List<Models.Cocktails> Cocktails { get; set; }
        public List<Models.Transactions> Transactions { get; set; }
        public List<int> earningsByCocktails { get; set; } // Bénéfices
        public List<int> profitabilityByCocktails { get; set; } // Rentabilités

        public StatsModel(DataAccess.Interfaces.ICocktailsRepository cocktailsRepository, DataAccess.Interfaces.ITransactionsRepository transactionRepository)
        {
            _cocktailsRepository = cocktailsRepository;
            _transactionsRepository = transactionRepository;

            earningsByCocktails = new List<int>();
            profitabilityByCocktails = new List<int>();
        }

        private void CalculateEachEarnings()
        {
            
        }

        private void CalculateEachProfitability()
        {

        }

        public void OnGetAsync()
        {
            Cocktails = _cocktailsRepository.GetCocktails();
            Transactions = _transactionsRepository.GetTransactions();
            CalculateEachEarnings();
            CalculateEachProfitability();
        }
    }
}