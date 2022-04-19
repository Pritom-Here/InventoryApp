namespace InventoryApp.Models
{
    public static class RolesAndPolicies
    {
        public static class Roles
        {
            public const string Administrator = "Administrator";
            public const string Cashier = "Cashier";
        }
        
        public static class Policies
        {
            public const string RequireAdmin = "RequireAdmin";
            public const string RequireCashier = "RequireCashier";
        }
    }
}
