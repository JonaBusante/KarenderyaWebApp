using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KarenderyaWebApp.Pages.Admin.Cashier
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToPage("/Forbidden");
            }

            // Allow access if the session exists
            return Page();
        }
    }
}
