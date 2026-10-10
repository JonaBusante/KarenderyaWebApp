namespace KarenderyaWebApp.Model;

public class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int FoodItemId { get; set; }

    public int Quantity { get; set; }

    public Order Order { get; set; } = null!;
    public decimal UnitPrice { get; set; }

    public FoodItem FoodItem { get; set; } = null!;
}
