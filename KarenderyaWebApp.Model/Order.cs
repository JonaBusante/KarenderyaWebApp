namespace KarenderyaWebApp.Model;

public class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    public int CashierId { get; set; }

    public decimal TotalAmount { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();
}
