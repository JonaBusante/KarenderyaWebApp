using KarenderyaWebApp.Model;
using KarenderyaWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KarenderyaWebApp.Pages.Cashier.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _db;

        public IndexModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public int OrdersToday { get; set; }
        public decimal SalesToday { get; set; }
        public int AvailableFoodItems { get; set; }

        public List<FoodItem> FoodItems { get; set; } = new();
        public List<Order> RecentOrders { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Redirect users who are not logged in
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToPage("/Forbidden");
            }

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            // Today's order count
            OrdersToday = await _db.Orders.CountAsync(o => o.OrderDate >= today && o.OrderDate < tomorrow);

            // Today's sales
            SalesToday = await _db.Orders.Where(o => o.OrderDate >= today && o.OrderDate < tomorrow).SumAsync(o => (decimal?)o.TotalAmount) ?? 0m;

            // Food items currently available and in stock
            AvailableFoodItems = await _db.FoodItems.CountAsync(f => f.IsAvailable && f.AvailableQuantity > 0);

            // Load all menu items for the food availability table
            FoodItems = await _db.FoodItems.AsNoTracking().OrderBy(f => f.Name).ToListAsync();

            RecentOrders = await _db.Orders.AsNoTracking().Include(order => order.OrderItems).ThenInclude(item => item.FoodItem).OrderByDescending(order => order.OrderDate).Take(5).ToListAsync();

            return Page();
        }
    }
}
