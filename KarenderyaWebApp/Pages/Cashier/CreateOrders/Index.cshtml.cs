using KarenderyaWebApp.Services;
using KarenderyaWebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KarenderyaWebApp.Pages.Cashier.CreateOrders
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDBContext _db;

        public IndexModel(ApplicationDBContext db)
        {
            _db = db;
        }

        public List<FoodItem> FoodItems { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> Quantities { get; set; } = new();

        public Order? CompletedOrder { get; set; }

        public List<OrderItem> CompletedOrderItems { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? receiptId)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")) || HttpContext.Session.GetString("Role") != "Cashier")
            {
                return RedirectToPage("/Forbidden");
            }

            await LoadFoodItemsAsync();

            if (receiptId.HasValue)
            {
                CompletedOrder = await _db.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == receiptId.Value);

                if (CompletedOrder != null)
                {
                    CompletedOrderItems = await _db.OrderItems.AsNoTracking().Include(i => i.FoodItem).Where(i => i.OrderId == receiptId.Value).ToListAsync();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")) || HttpContext.Session.GetString("Role") != "Cashier")
            {
                return RedirectToPage("/Forbidden");
            }

            int? cashierId = HttpContext.Session.GetInt32("UserId");

            if (cashierId == null)
            {
                return RedirectToPage("/Forbidden");
            }

            if (Quantities == null || Quantities.Any(q => q.Value < 0))
            {
                ModelState.AddModelError("", "Quantities cannot be negative.");
                await LoadFoodItemsAsync();
                return Page();
            }

            var selectedItems = Quantities.Where(q => q.Value > 0).ToDictionary(q => q.Key, q => q.Value);

            if (selectedItems.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one food item.");
                await LoadFoodItemsAsync();
                return Page();
            }

            var strategy = _db.Database.CreateExecutionStrategy();

            try
            {
                var orderId = await strategy.ExecuteAsync(async () =>
                {
                    await using var transaction = await _db.Database.BeginTransactionAsync();

                    var foodIds = selectedItems.Keys.ToList();

                    var foods = await _db.FoodItems.Where(f => foodIds.Contains(f.FoodItemId)).ToListAsync();

                    if (foods.Count != foodIds.Count)
                    {
                        throw new InvalidOperationException("One or more food items could not be found.");
                    }

                    foreach (var food in foods)
                    {
                        int quantity = selectedItems[food.FoodItemId];

                        if (!food.IsAvailable || food.AvailableQuantity <= 0 || quantity > food.AvailableQuantity)
                        {
                            throw new InvalidOperationException( $"Insufficient stock or unavailable item: {food.Name}.");
                        }
                    }

                    decimal totalAmount = foods.Sum(food => food.Price * selectedItems[food.FoodItemId]);

                    var order = new Order
                    {
                        OrderDate = DateTime.Now,
                        CashierId = cashierId.Value,
                        TotalAmount = totalAmount
                    };

                    _db.Orders.Add(order);
                    await _db.SaveChangesAsync();

                    foreach (var food in foods)
                    {
                        int quantity = selectedItems[food.FoodItemId];

                        _db.OrderItems.Add(new OrderItem
                        {
                            OrderId = order.OrderId,
                            FoodItemId = food.FoodItemId,
                            Quantity = quantity,
                            UnitPrice = food.Price
                        });

                        food.AvailableQuantity -= quantity;

                        if (food.AvailableQuantity <= 0)
                        {
                            food.IsAvailable = false;
                        }
                    }

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return order.OrderId;
                });

                return RedirectToPage(new { receiptId = orderId });
            }
            catch (InvalidOperationException ex)
            {
                _db.ChangeTracker.Clear();

                ModelState.AddModelError("", ex.Message);
                await LoadFoodItemsAsync();
                return Page();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);

                _db.ChangeTracker.Clear();

                ModelState.AddModelError("", "Unable to save the order. Please try again.");

                await LoadFoodItemsAsync();
                return Page();
            }
        }

        private async Task LoadFoodItemsAsync()
        {
            FoodItems = await _db.FoodItems.AsNoTracking().Where(f => f.IsAvailable && f.AvailableQuantity > 0).OrderBy(f => f.Name).ToListAsync();
        }
    }
}
