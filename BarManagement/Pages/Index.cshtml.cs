﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

    public class IndexModel : PageModel
    {
        private readonly DataAccess.Interfaces.IStocksRepository _stocksRepository;
        private readonly DataAccess.Interfaces.IDrinksRepository _drinksRepository;
        public List<Models.Stocks> Stocks { get; set; }
        public List<Models.Drinks> Drinks { get; set; }
        public List<SelectListItem> DrinksOptions { get; set; }

        public Boolean isBtnUpdateCliked { get; set; }

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


        public IndexModel(DataAccess.Interfaces.IStocksRepository stocksRepository, DataAccess.Interfaces.IDrinksRepository drinksRepository)
        {
            _stocksRepository = stocksRepository;
            _drinksRepository = drinksRepository;
        }

        public void OnGetAsync()
        {
            Drinks = _drinksRepository.GetDrinks();
            Stocks = _stocksRepository.GetStocks();
            var CopyDrinks = new List<Models.Drinks>(Drinks);
            DrinksOptions = CopyDrinks.Select(drink => new SelectListItem { Value = drink.Id.ToString(), Text = drink.Name }).ToList();
            isBtnUpdateCliked = false;
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
            var stock = await _stocksRepository.Insert(new Models.Stocks() { DrinkId = drink.Id, Price = Double.Parse((FormStock.PRICE).Replace('.',',')), Quantity = Int32.Parse(FormStock.QUANTITY)});
            stock.Drink = drink;
            return Redirect("./Index");
        }

        protected void btnUpdateStock_Click(object sender, EventArgs e)
        {
            isBtnUpdateCliked = true;
        }
    }
}