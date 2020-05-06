using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BarManagement.Pages
{
    public static class SessionExtensions
    {
        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
    public class CocktailsModel : PageModel
    {
        private readonly DataAccess.Interfaces.IDrinksRepository _drinksRepository;
        private readonly DataAccess.Interfaces.ICocktailsRepository _cocktailsRepository;

        public List<Models.Cocktails> Cocktails { get; set; }
        public List<Models.Drinks> Drinks { get; set; }
        public List<SelectListItem> DrinksOptions { get; set; }
        [BindProperty]
        public List<Models.CocktailsComposition> Compositions { get; set; }
        public CocktailsModel(DataAccess.Interfaces.ICocktailsRepository cocktailsRepository, DataAccess.Interfaces.IDrinksRepository drinksRepository)
        {
            _cocktailsRepository = cocktailsRepository;
            _drinksRepository = drinksRepository;
            Compositions = new List<Models.CocktailsComposition>();
        }

        public class FormCocktailModel
        {
            [BindProperty]
            [Required]
            public string NAME { get; set; }
            [BindProperty]
            [Required]
            public string PRICE { get; set; }
        }

        [BindProperty]
        public FormCocktailModel FormCocktail { get; set; }

        public void OnGetAsync()
        {
            Drinks = _drinksRepository.GetDrinks();
            Cocktails = _cocktailsRepository.GetCocktails();
        }
        public async Task<IActionResult> OnPostCocktails()
        {
            ModelState.MarkAllFieldsAsSkipped();
            if (!TryValidateModel(FormCocktail, nameof(FormCocktail)))
            {
                return Page();
            }
            await _cocktailsRepository.Insert(new Models.Cocktails() { Name = FormCocktail.NAME, PriceToSell = Double.Parse(FormCocktail.PRICE) });
            return Redirect("./Cocktails");
        }
        public void OnPostAdd()
        {
            Drinks = _drinksRepository.GetDrinks();
            var CopyDrinks = new List<Models.Drinks>(Drinks);
            DrinksOptions = CopyDrinks.Select(drink => new SelectListItem { Value = drink.Id.ToString(), Text = drink.Name }).ToList();
            Compositions.Add(new Models.CocktailsComposition());
        }
        public void OnPostRemove(int index)
        {
            Compositions.RemoveAt(index);
        }

        public async Task<IActionResult> OnPostUpdateCocktail(long id)
        {
            ModelState.MarkAllFieldsAsSkipped();
            Cocktails = _cocktailsRepository.GetCocktails();
            if (!TryValidateModel(FormCocktail, nameof(FormCocktail)))
            {
                return Page();
            }
            Models.Cocktails coktail = Cocktails.First(cocktail => cocktail.Id == id);
            var cocktail = await _cocktailsRepository.Update(new Models.Cocktails() { Name = FormCocktail.NAME, PriceToSell = Double.Parse(FormCocktail.PRICE)});
            return Redirect("./Cocktails");
        }

        public async Task<IActionResult> OnPostRemoveCocktail(long id)
        {
            await _cocktailsRepository.Delete(id);
            return Redirect("./Cocktails");
        }

    }
}