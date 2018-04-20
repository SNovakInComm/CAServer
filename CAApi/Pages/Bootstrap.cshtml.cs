using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CAApi.Pages
{
    public class BootstrapModel : PageModel
    {
        [BindProperty]
        public string BootstrapToken { get; set; }

        public void OnGet()
        {
            return;
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Main");
        }
    }
}