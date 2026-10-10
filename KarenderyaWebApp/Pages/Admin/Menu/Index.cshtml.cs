
using KarenderyaWebApp.Model;
using KarenderyaWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KarenderyaWebApp.Pages.Admin.Menu;

public class IndexModel : PageModel
{
    private readonly ApplicationDBContext _db;

    public IndexModel(ApplicationDBContext db)
    {
        _db = db;
    }

    public List<FoodItem> FoodItems { get; set; } = new();

    [BindProperty]
    public FoodItem NewFood { get; set; } = new();


    public async Task<IActionResult> OnGetAsync()
    {
        // Check if the user is logged in
        if (HttpContext.Session.GetString("Role") != "Admin")
        {
            return RedirectToPage("/Forbidden");
        }

        await LoadFoodItemsAsync();

        return Page();
    }


    public async Task<IActionResult> OnPostAddAsync()
    {
        if (string.IsNullOrWhiteSpace(NewFood.Name) ||
            NewFood.Price <= 0 ||
            NewFood.AvailableQuantity < 0)
        {
            ModelState.AddModelError("", "Enter a valid name, price, and stock quantity.");
            await LoadFoodItemsAsync();
            return Page();
        }

        NewFood.Name = NewFood.Name.Trim();
        NewFood.IsAvailable = NewFood.AvailableQuantity > 0;

        _db.FoodItems.Add(NewFood);
        await _db.SaveChangesAsync();

        TempData["Success"] = "Food item added successfully.";

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUpdateAsync(int foodItemId, string name, decimal price, int availableQuantity)
    {
        if (string.IsNullOrWhiteSpace(name) || price <= 0 || availableQuantity < 0)
        {
            TempData["Error"] = "Enter valid food details.";
            return RedirectToPage();
        }

        var food = await _db.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == foodItemId);

        if (food == null)
        {
            return NotFound();
        }

        food.Name = name.Trim();
        food.Price = price;
        food.AvailableQuantity = availableQuantity;

        if (availableQuantity == 0)
        {
            food.IsAvailable = false;
        }

        await _db.SaveChangesAsync();

        TempData["Success"] = "Food item updated successfully.";

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleAsync(int id)
    {
        var food = await _db.FoodItems.FirstOrDefaultAsync(f => f.FoodItemId == id);

        if (food == null)
        {
            return NotFound();
        }

        if (!food.IsAvailable && food.AvailableQuantity <= 0)
        {
            TempData["Error"] = "Add stock before making this item available.";
            return RedirectToPage();
        }

        food.IsAvailable = !food.IsAvailable;

        await _db.SaveChangesAsync();

        TempData["Success"] = "Food availability updated.";

        return RedirectToPage();
    }

    private async Task LoadFoodItemsAsync()
    {
        FoodItems = await _db.FoodItems.AsNoTracking().OrderBy(f => f.Name).ToListAsync();
    }
}
