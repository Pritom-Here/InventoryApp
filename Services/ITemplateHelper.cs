namespace InventoryApp.Services
{
    public interface ITemplateHelper
    {
        Task<string> GetHtmlTemplateAsStringAsync<T>(string viewName, T model);
    }
}
