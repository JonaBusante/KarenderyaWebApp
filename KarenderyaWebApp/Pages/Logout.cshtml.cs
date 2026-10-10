using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KarenderyaWebApp.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPost()
        {
            HttpContext.Session.Clear();

            return RedirectToPage("/Index");
        }
    }
}
