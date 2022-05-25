namespace InventoryApp.Services
{
    public class RandomCodeGenerator : IRandomCodeGenerator
    {
        public string GenerateRandomCode()
        {
            var random = new Random();
            const string alphabets = "AABBCCDDEEFFGGHHIIJJKKLLMMNNOOPPQQRRSSTTUUVVWWXXYYZZ";
            const string numbers = "11223344556677889900";
            string randomAlphabets = new string(Enumerable.Repeat(alphabets, 4).Select(s => s[random.Next(s.Length)]).ToArray());
            string randomNumbers = new string(Enumerable.Repeat(alphabets, 5).Select(s => s[random.Next(s.Length)]).ToArray());

            var randomCode = randomAlphabets + "-" + randomNumbers;
            return randomCode;
        }
    }
}
