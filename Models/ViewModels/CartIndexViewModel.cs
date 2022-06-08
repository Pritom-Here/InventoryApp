namespace InventoryApp.Models.ViewModels
{
    public class CartIndexViewModel
    {
        public int ItemsCount { get; set; }
        public int TotalItemsCount { get; set; }

        public IEnumerable<Cart> CartItems { get; set; }

        public double SubTotal { get; set; }
        public double Vat { get; set; }
        public double Discount { get; set; }
        public double DeliveryCharge { get; set; }
        public double Total { get; set; }
    }
}
