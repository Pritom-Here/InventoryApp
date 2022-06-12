namespace InventoryApp.Models.ViewModels
{
    public class POSViewModel
    {
        public IEnumerable<Category> PrimaryCategories { get; set; } = new List<Category>();
        public IEnumerable<Category> SecondaryCategories { get; set; } = new List<Category>();
        public IEnumerable<Category> TertiaryCategories { get; set; } = new List<Category>();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        public string PrimaryCategoryId { get; set; }
        public string SecondaryCategoryId { get; set; }
        public string TertiaryCategoryId { get; set; }

        public int CartItemsCount { get; set; }
    }
}
