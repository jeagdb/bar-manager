using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BarManagement.Pages
{
    public class TransactionsModel : PageModel
    {
        public class FormBalanceModel
        {
            [BindProperty]
            [Required]
            public string BALANCE { get; set; }
        }

        private readonly DataAccess.Interfaces.ITransactionsRepository _transactionsRepository;
        public List<Models.Transactions> Transactions { get; set; }
        public List<SelectListItem> TransactionsOptions { get; set; }
        public Double currentMoney { get; set; }

        public TransactionsModel(DataAccess.Interfaces.ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        [BindProperty]
        public FormBalanceModel FormBalance { get; set; }

        public void OnGetAsync()
        {
            Transactions = _transactionsRepository.GetTransactions();
            var CopyTransactions = new List<Models.Transactions>(Transactions);
            TransactionsOptions = CopyTransactions.Select(transaction => new SelectListItem { 
                Value = transaction.Id.ToString(), Text = transaction.Value.ToString() }).ToList();
            foreach (var item in Transactions)
            {
                currentMoney += item.Value;
            }
        }

        public async Task<IActionResult> OnPostTransactions()
        {
            ModelState.MarkAllFieldsAsSkipped();
            if (!TryValidateModel(FormBalance, nameof(FormBalance)))
            {
                return Page();
            }
            await _transactionsRepository.Insert(new Models.Transactions() { SellDate = DateTime.Now, Value = Double.Parse((FormBalance.BALANCE).Replace('.', ',')) });
            return Redirect("./Transactions");
        }
    }
}