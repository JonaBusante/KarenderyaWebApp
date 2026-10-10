
using KarenderyaWebApp.Model;
using KarenderyaWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KarenderyaWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDBContext _db;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                string? role = HttpContext.Session.GetString("Role");

                return role switch
                {
                    "Admin" => RedirectToPage("/Admin/Dashboard/Index"),
                    "Cashier" => RedirectToPage("/Cashier/Dashboard/Index"),
                    _ => RedirectToPage("/Forbidden")
                };
            }

            return Page();
        }

    }
}