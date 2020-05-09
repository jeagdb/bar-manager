using BarManagement.DataAccess.EfModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BarManagement.Pages
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary MarkAllFieldsAsSkipped(this ModelStateDictionary modelState)
        {
            foreach (var state in modelState.Select(x => x.Value))
            {
                state.Errors.Clear();
                state.ValidationState = ModelValidationState.Skipped;
            }
            return modelState;
        }
    }
    public class StocksModel : PageModel
    {
        private readonly DataAccess.Interfaces.IStocksRepository _stocksRepository;
        private readonly DataAccess.Interfaces.ITransactionsRepository _transactionsRepository;
        private readonly DataAccess.Interfaces.IDrinksRepository _drinksRepository;
        private readonly DataAccess.Interfaces.ICocktailsCompositionRepository _cocktailsCompositionRepository;
        private readonly DataAccess.Interfaces.ICocktailsRepository _cocktailsRepository;
        private List<string> Categories = new List<string> { "Soft", "Alcool" };

        public List<Models.Stocks> Stocks { get; set; }
        public List<Models.Drinks> Drinks { get; set; }
        public List<Models.Cocktails> Cocktails { get; set; }
        public List<Models.CocktailsComposition> CocktailsCompositions { get; set; }
        public List<SelectListItem> DrinksOptions { get; set; }
        public List<SelectListItem> CategoryOptions { get; set; }


        public bool isDelete { get; set; }
        public class FormDrinkModel
        {
            [BindProperty]
            [Required]
            public string NAME { get; set; }
            [BindProperty]
            [Required]
            public string BRAND { get; set; }
        }
        public class FormStockModel
        {
            [BindProperty]
            [Required]
            public string CAPACITY { get; set; }

            [BindProperty]
            [Required]
            public string NUMBER { get; set; }
            [BindProperty]
            [Required]
            public string PRICE { get; set; }
        }

        [BindProperty]
        public FormDrinkModel FormDrink { get; set; }


        [BindProperty]
        public FormStockModel FormStock { get; set; }


        public StocksModel(DataAccess.Interfaces.IStocksRepository stocksRepository,
            DataAccess.Interfaces.IDrinksRepository drinksRepository,
            DataAccess.Interfaces.ITransactionsRepository transactionsRepository,
            DataAccess.Interfaces.ICocktailsCompositionRepository cocktailsCompositionRepository,
            DataAccess.Interfaces.ICocktailsRepository cocktailsRepository)
        {
            _stocksRepository = stocksRepository;
            _drinksRepository = drinksRepository;
            _transactionsRepository = transactionsRepository;
            _cocktailsCompositionRepository = cocktailsCompositionRepository;
            _cocktailsRepository = cocktailsRepository;
        }

        public void OnGetAsync()
        {
            Drinks = _drinksRepository.GetDrinks();
            Stocks = _stocksRepository.GetStocks();
            var CopyDrinks = new List<Models.Drinks>(Drinks);
            DrinksOptions = CopyDrinks.Select(drink => new SelectListItem { Value = drink.Id.ToString(), Text = drink.Name }).ToList();
            CategoryOptions = Categories.Select(category => new SelectListItem { Value = category, Text = category }).ToList();

            isDelete = true;
        }

        public async Task<IActionResult> OnPostDrinks()
        {
            ModelState.MarkAllFieldsAsSkipped();
            if (!TryValidateModel(FormDrink, nameof(FormDrink)))
            {
                return Redirect("./_ViewStocks");
            }
            await _drinksRepository.Insert(new Models.Drinks() { Name = FormDrink.NAME, Brand = FormDrink.BRAND, Category = Request.Form["categorySelected"] });
            return Redirect("./_ViewStocks");
        }

        public async Task<IActionResult> OnPostStocks()
        {
            ModelState.MarkAllFieldsAsSkipped();
            Drinks = _drinksRepository.GetDrinks();
            Cocktails = _cocktailsRepository.GetCocktails();
            if (!TryValidateModel(FormStock, nameof(FormStock)) || Drinks == null)
            {
                return Redirect("./_ViewStocks");
            }
            var drinkId = Int32.Parse(Request.Form["drinkSelected"]);
            Models.Drinks drink = Drinks.First(drink => drink.Id == drinkId);
            long quantity = calculateTotalQuantity(Int32.Parse(FormStock.NUMBER), Int32.Parse(FormStock.CAPACITY));
            double pricePerCl = calculatePricePerCl(Double.Parse((FormStock.PRICE).Replace('.', ',')), Int32.Parse(FormStock.CAPACITY));

            foreach (Models.Cocktails cocktail in Cocktails)
            {
                CocktailsCompositions = _cocktailsCompositionRepository.getCompositionByCocktailId(cocktail.Id);
                foreach (Models.CocktailsComposition cocktailsComposition in CocktailsCompositions)
                {
                    if (cocktailsComposition.DrinkId == drinkId)
                    {
                        Double cost = cocktailsComposition.Quantity * pricePerCl;
                        cocktail.Cost += cost;
                        await _cocktailsRepository.Update(cocktail);
                    }
                }
            }

            var stock = await _stocksRepository.Insert(new Models.Stocks() { DrinkId = drink.Id, Price = pricePerCl, Quantity = quantity });
            var transaction = await _transactionsRepository.Insert(new Models.Transactions() { SellDate = DateTime.Now, Value = -(Double.Parse((FormStock.PRICE).Replace('.', ',')) * Int32.Parse(FormStock.NUMBER)), CocktailId = null });
            stock.Drink = drink;
            return Redirect("./_ViewStocks");
        }

        public async Task<IActionResult> OnPostUpdateStock(long id)
        {
            ModelState.MarkAllFieldsAsSkipped();
            Stocks = _stocksRepository.GetStocks();
            Cocktails = _cocktailsRepository.GetCocktails();
            if (!TryValidateModel(FormStock, nameof(FormStock)) || Stocks == null)
            {
                return Redirect("./_ViewStocks");
            }
            Models.Stocks stock = Stocks.First(stock => stock.Id == id);
            long quantity = calculateTotalQuantity(Int32.Parse(FormStock.NUMBER), Int32.Parse(FormStock.CAPACITY));
            double pricePerCl = calculatePricePerCl(Double.Parse((FormStock.PRICE).Replace('.', ',')), Int32.Parse(FormStock.CAPACITY));

            foreach (Models.Cocktails cocktail in Cocktails)
            {
                CocktailsCompositions = _cocktailsCompositionRepository.getCompositionByCocktailId(cocktail.Id);
                foreach (Models.CocktailsComposition cocktailsComposition in CocktailsCompositions)
                {
                    if (cocktailsComposition.DrinkId == stock.DrinkId)
                    {
                        Double cost = cocktailsComposition.Quantity * pricePerCl;
                        cocktail.Cost += cost;
                        await _cocktailsRepository.Update(cocktail);
                    }
                }
            }

            stock.Price = pricePerCl;

            double subQuantity = Math.Abs((Double) (quantity - stock.Quantity) / Double.Parse(FormStock.CAPACITY)); 
            stock.Quantity = quantity;
            await _stocksRepository.Update(stock);
            var updatedTransactions = await _transactionsRepository.Insert(new Models.Transactions()
            { SellDate = DateTime.Now, Value = - subQuantity * Double.Parse((FormStock.PRICE)), CocktailId = null });

            return Redirect("./_ViewStocks");
        }

        public async Task<IActionResult> OnPostRemoveStock(long id)
        {
            await _stocksRepository.Delete(id);
            return Redirect("./_ViewStocks");
        }
        public async Task<IActionResult> OnPostRemoveDrink(long id)
        {
            Drinks = _drinksRepository.GetDrinks();
            var drinkUtil = Drinks.First(drink => drink.Id == id);
            isDelete = await _drinksRepository.Delete(id);
            if (isDelete == false)
            {
                TempData["error"] = "Attention, suppression impossible : il existe un stock de cette boisson : " + drinkUtil.Name;
            }
            return Redirect("./_ViewStocks");
        }
        
        // Retourne le prix au cl
        public double calculatePricePerCl(double priceQuantity, long quantity)
        {
            return Math.Round(priceQuantity / quantity, 2);
        }

        // Retourne la quantité totale de cl du stock ajouté
       public long calculateTotalQuantity(long capacity, long number)
        {
            return capacity * number;
        }
    }
}