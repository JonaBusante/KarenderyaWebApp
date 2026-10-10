
namespace KarenderyaWebApp.Model;

public class FoodItem
{
    public int FoodItemId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int AvailableQuantity { get; set; }

    public bool IsAvailable { get; set; } = true;
}
