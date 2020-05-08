using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarManagement.Models;
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
        public List<List<Models.Cocktails>> CocktailsSortedByCategory { get; set; }

        public StatsModel(DataAccess.Interfaces.ICocktailsRepository cocktailsRepository, DataAccess.Interfaces.ITransactionsRepository transactionRepository)
        {
            _cocktailsRepository = cocktailsRepository;
            _transactionsRepository = transactionRepository;
        }

        public void OnGetAsync()
        {
            Cocktails = _cocktailsRepository.GetCocktails();
            Transactions = _transactionsRepository.GetTransactions();
            CocktailsSortedByCategory = _cocktailsRepository.GetCocktailsSortedByCategory();
        }

        public double[] CurrentCocktailStats(int id)
        {
            double nbSoldGlasses = 0;
            double earning = 0;
            double totalEarnings = 0;
            double profitability = 0;

            double priceToSell = Cocktails[id].PriceToSell;
            double cost = Cocktails[id].Cost;

            List<Transactions> cocktailTransactions = _transactionsRepository.GetTransactionsByCocktail(Cocktails[id].Id);
            cocktailTransactions.ForEach(t => {
                double div = t.Value / priceToSell;
                nbSoldGlasses += div;
            });

            earning = priceToSell - cost;
            totalEarnings = earning * nbSoldGlasses;
            profitability = priceToSell / cost;
            return new double[]{ nbSoldGlasses, earning, totalEarnings, profitability };
        }

        public string GetCategoryIconPath(string categorie)
        {
            switch (categorie)
            {
                case "Alcool":
                case "alcool":
                    return "~/Assets/alcool.png";
                case "Soft":
                case "soft":
                    return "~/Assets/soft.png";
                case "Cocktail":
                case "cocktail":
                    return "~/Assets/cocktail.png";
                case "Cocktail sans alcool":
                case "cocktail sans alcool":
                    return "~/Assets/cocktail-sans-alcool.png";
                default:
                    return "";
            }
        }
    }
}