using System.ComponentModel.DataAnnotations;

namespace InventoryApp.Models
{
    public class OrderDetail: BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public double Quantity { get; set; }
        
        [Required]
        public double ItemTotal { get; set; }
        
        [Required]
        public string OrderId { get; set; }
        public Order Order { get; set; }
        
        [Required]
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
