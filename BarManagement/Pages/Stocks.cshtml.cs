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
        private List<string> Categories = new List<string> { "Soft", "Alcool" };

        public List<Models.Stocks> Stocks { get; set; }
        public List<Models.Drinks> Drinks { get; set; }
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
            DataAccess.Interfaces.ITransactionsRepository transactionsRepository)
        {
            _stocksRepository = stocksRepository;
            _drinksRepository = drinksRepository;
            _transactionsRepository = transactionsRepository;
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
                return Redirect("./Stocks");
            }
            await _drinksRepository.Insert(new Models.Drinks() { Name = FormDrink.NAME, Brand = FormDrink.BRAND, Category = Request.Form["categorySelected"] });
            return Redirect("./Stocks");
        }

        public async Task<IActionResult> OnPostStocks()
        {
            ModelState.MarkAllFieldsAsSkipped();
            Drinks = _drinksRepository.GetDrinks();
            if (!TryValidateModel(FormStock, nameof(FormStock)) || Drinks == null)
            {
                return Redirect("./Stocks");
            }
            var drinkId = Int32.Parse(Request.Form["drinkSelected"]);
            Models.Drinks drink = Drinks.First(drink => drink.Id == drinkId);
            long quantity = calculateTotalQuantity(Int32.Parse(FormStock.NUMBER), Int32.Parse(FormStock.CAPACITY));
            double pricePerCl = calculatePricePerCl(Double.Parse((FormStock.PRICE).Replace('.', ',')), Int32.Parse(FormStock.CAPACITY));

            var stock = await _stocksRepository.Insert(new Models.Stocks() { DrinkId = drink.Id, Price = pricePerCl, Quantity = quantity });
            var transaction = await _transactionsRepository.Insert(new Models.Transactions() { SellDate = DateTime.Now, Value = -(Double.Parse((FormStock.PRICE).Replace('.', ',')) * Int32.Parse(FormStock.NUMBER)), CocktailId = null });
            stock.Drink = drink;
            return Redirect("./Stocks");
        }

        public async Task<IActionResult> OnPostUpdateStock(long id)
        {
            ModelState.MarkAllFieldsAsSkipped();
            Stocks = _stocksRepository.GetStocks();
            if (!TryValidateModel(FormStock, nameof(FormStock)) || Stocks == null)
            {
                return Redirect("./Stocks");
            }
            Models.Stocks stock = Stocks.First(stock => stock.Id == id);

            long quantity = calculateTotalQuantity(Int32.Parse(FormStock.NUMBER), Int32.Parse(FormStock.CAPACITY));
            double pricePerCl = calculatePricePerCl(Double.Parse((FormStock.PRICE).Replace('.', ',')), Int32.Parse(FormStock.CAPACITY));
            stock.Price = pricePerCl;
            stock.Quantity = quantity;

            await _stocksRepository.Update(stock);

            return Redirect("./Stocks");
        }

        public async Task<IActionResult> OnPostRemoveStock(long id)
        {
            await _stocksRepository.Delete(id);
            return Redirect("./Stocks");
        }
        public async Task<IActionResult> OnPostRemoveDrink(long id)
        {
            isDelete = await _drinksRepository.Delete(id);
            return Redirect("./Stocks");
        }

        public double calculatePricePerCl(double priceQuantity, long quantity)
        {
            return Math.Round(priceQuantity / quantity, 3);
        }

       public long calculateTotalQuantity(long capacity, long number)
        {
            return capacity * number;
        }
    }
}