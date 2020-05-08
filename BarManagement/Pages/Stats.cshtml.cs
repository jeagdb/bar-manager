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
        public List<List<double>> NbSoldGlassesSortedByCategory { get; set; }

        public StatsModel(DataAccess.Interfaces.ICocktailsRepository cocktailsRepository, DataAccess.Interfaces.ITransactionsRepository transactionRepository)
        {
            _cocktailsRepository = cocktailsRepository;
            _transactionsRepository = transactionRepository;
        }

        // Retourne une liste de nombre de verres dont la position dans la liste correspond
        // à la position du cocktail dans la liste CocktailsSortedByCategory
        public List<List<double>> CalculateNbGlassesForEachCocktail()
        {
            List<List<double>> nbGlassesList = new List<List<double>>();
            foreach (var cocktailsByCategory in CocktailsSortedByCategory)
            {
                List<double> nbGlassesListByCategory = new List<double>();
                foreach (var cocktail in cocktailsByCategory)
                {
                    double nbSoldGlasses = 0;
                    List<Transactions> cocktailTransactions = _transactionsRepository.GetTransactionsByCocktail(cocktail.Id);
                    cocktailTransactions.ForEach(t => {
                        double div = t.Value / cocktail.PriceToSell;
                        nbSoldGlasses += div;
                    });
                    nbGlassesListByCategory.Add(nbSoldGlasses);
                }
                nbGlassesList.Add(nbGlassesListByCategory);
            }
            return nbGlassesList;
        }

        public void OnGetAsync()
        {
            Cocktails = _cocktailsRepository.GetCocktails();
            Transactions = _transactionsRepository.GetTransactions();
            CocktailsSortedByCategory = _cocktailsRepository.GetCocktailsSortedByCategory();
            NbSoldGlassesSortedByCategory = CalculateNbGlassesForEachCocktail();
        }

        public double[] CurrentCocktailStats(int indexI, int indexJ)
        {
            Cocktails currentCocktails = CocktailsSortedByCategory[indexI][indexJ];

            double priceToSell = currentCocktails.PriceToSell;
            double cost = currentCocktails.Cost;

            double nbSoldGlasses = NbSoldGlassesSortedByCategory[indexI][indexJ];
            double earning = priceToSell - cost;
            double totalEarnings = earning * nbSoldGlasses;
            double profitability = priceToSell / cost;
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

        public (double, double) FindMaxNbSoldGlassesByCategory(int indexI)
        {
            List<Cocktails> cocktails = CocktailsSortedByCategory[indexI];
            List<double> nbSoldGlassesList = NbSoldGlassesSortedByCategory[indexI];

            if (nbSoldGlassesList.Count == 1)
            {
                if (nbSoldGlassesList[0] == 0)
                    return (-1, -1);
                return (nbSoldGlassesList[0], -1);
            }

            if (nbSoldGlassesList.Count == 2)
            {
                if (nbSoldGlassesList[1] != 0)
                    return (nbSoldGlassesList[0], nbSoldGlassesList[1]);
                return (nbSoldGlassesList[0], -1);
            }

            double maxNbSold = nbSoldGlassesList[0];
            double secondNbSold = nbSoldGlassesList[1];
            for (int i = 1; i < nbSoldGlassesList.Count; i++)
            {
                if (nbSoldGlassesList[i] > maxNbSold)
                {
                    secondNbSold = maxNbSold;
                    maxNbSold = nbSoldGlassesList[i];
                }
                else if (nbSoldGlassesList[i] > secondNbSold)
                    secondNbSold = nbSoldGlassesList[i];
            }

            return (maxNbSold, secondNbSold);
        }

        public void FindSmallestNbGlassesSoldByCategory(int indexI)
        {

        }
    }
}