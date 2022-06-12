namespace InventoryApp.Models
{
    public class Cart
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double UnitPrice { get; set; }
        public double ItemTotal { get; set; }
    }
}
