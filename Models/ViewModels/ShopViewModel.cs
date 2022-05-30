namespace InventoryApp.Models.ViewModels
{
    public class ShopViewModel
    {
        public ShopViewModel()
        {
            Prices = new List<PriceRangeViewModel>
            {
                new PriceRangeViewModel{Id = 1, MinPrice = 0, MaxPrice = 250},
                new PriceRangeViewModel{Id = 2, MinPrice = 250, MaxPrice = 500},
                new PriceRangeViewModel{Id = 3, MinPrice = 500, MaxPrice = 1000},
                new PriceRangeViewModel{Id = 4, MinPrice = 1000, MaxPrice = 2500},
                new PriceRangeViewModel{Id = 5, MinPrice = 2500, MaxPrice = 5000},
                new PriceRangeViewModel{Id = 6, MinPrice = 5000, MaxPrice = 10000}
            };
        }

        public IEnumerable<Category> PrimaryCategories { get; set; }
        public IEnumerable<Category> SecondaryCategories { get; set; }
        public IEnumerable<Category> TertiaryCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<PriceRangeViewModel> Prices { get; set; }
    }
}
