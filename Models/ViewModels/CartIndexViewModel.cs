namespace InventoryApp.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<Cart> CartItems { get; set; }
        public double SubTotal { get; set; }
        public double Vat { get; set; } = 15;
        public double Discount { get; set; }
        public double DeliveryCharge { get; set; }
        public double Total { get; set; }
    }
}
