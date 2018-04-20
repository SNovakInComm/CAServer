using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CAApi.Utilities;

namespace CAApi.Pages
{
    public class IndexModel : PageModel
    {
        
        public IActionResult OnGet()
        {
            if(!Crypto.Validated)
            {
                return RedirectToPage("/Bootstrap");
            }

            return RedirectToPage("/Main");
        }

        public bool Validated
        {
            get { return Crypto.Validated; }
        }

        public string BaseURI
        {
            get { return "https://localhost:44321/Api"; }
        }
    }
}