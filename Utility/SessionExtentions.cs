using Newtonsoft.Json;

namespace InventoryApp.Utility
{
    public static class SessionExtentions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(
                            key, 
                            JsonConvert.SerializeObject(
                                                    value, 
                                                    Formatting.Indented, 
                                                    new JsonSerializerSettings()
                                                    {
                                                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                                                    }
                                                    )
                            );
        }


        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
