using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public List<Models.Stocks> Stocks { get; set; }
        public List<Models.Drinks> Drinks { get; set; }
        public List<SelectListItem> DrinksOptions { get; set; }

        public bool isDelete { get; set; }
        public class FormDrinkModel
        {
            [BindProperty]
            [Required]
            public string NAME { get; set; }
            [BindProperty]
            [Required]
            public string BRAND { get; set; }
            [BindProperty]
            [Required]

            public string CATEGORY { get; set; }
        }
        public class FormStockModel
        {
            [BindProperty]
            [Required]
            public string QUANTITY { get; set; }
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
            isDelete = true;
        }

        public async Task<IActionResult> OnPostDrinks()
        {
            ModelState.MarkAllFieldsAsSkipped();
            if (!TryValidateModel(FormDrink, nameof(FormDrink)))
            {
                return Page();
            }
            await _drinksRepository.Insert(new Models.Drinks() { Name = FormDrink.NAME, Brand = FormDrink.BRAND, Category = FormDrink.CATEGORY });
            return Redirect("./Index");
        }

        public async Task<IActionResult> OnPostStocks()
        {
            ModelState.MarkAllFieldsAsSkipped();
            Drinks = _drinksRepository.GetDrinks();
            if (!TryValidateModel(FormStock, nameof(FormStock)) || Drinks == null)
            {
                return Page();
            }
            var drinkId = Int32.Parse(Request.Form["drinkSelected"]);
            Models.Drinks drink = Drinks.First(drink => drink.Id == drinkId);
            var stock = await _stocksRepository.Insert(new Models.Stocks() { DrinkId = drink.Id, Price = Double.Parse((FormStock.PRICE).Replace('.', ',')), Quantity = Int32.Parse(FormStock.QUANTITY) });
            var transaction = await _transactionsRepository.Insert(new Models.Transactions() { SellDate = DateTime.Now, Value = - Double.Parse((FormStock.PRICE).Replace('.', ',')) });
            stock.Drink = drink;
            return Redirect("./Index");
        }

        public async Task<IActionResult> OnPostUpdateStock(long id)
        {
            ModelState.MarkAllFieldsAsSkipped();
            Drinks = _drinksRepository.GetDrinks();
            if (!TryValidateModel(FormStock, nameof(FormStock)) || Drinks == null)
            {
                return Page();
            }
            Models.Drinks drink = Drinks.First(drink => drink.Id == id);
            var stock = await _stocksRepository.Update(new Models.Stocks() { DrinkId = drink.Id, Price = Double.Parse((FormStock.PRICE).Replace('.', ',')), Quantity = Int32.Parse(FormStock.QUANTITY) });
            return Redirect("./Index");
        }

        public async Task<IActionResult> OnPostRemoveStock(long id)
        {
            await _stocksRepository.Delete(id);
            return Redirect("./Index");
        }
        public async Task<IActionResult> OnPostRemoveDrink(long id)
        {
            isDelete = await _drinksRepository.Delete(id);
            return Redirect("./Index");
        }
    }
}