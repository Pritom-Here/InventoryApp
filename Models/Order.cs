namespace InventoryApp.Models
{
    public class Order: BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TransactionCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public double SubTotal { get; set; }
        public double VatInPercentage { get; set; }
        public double Total { get; set; }
        public double PaidAmount { get; set; }

    }
}
