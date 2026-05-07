namespace Client
{
    public static class InMemoryTokenStore
    {
        private static readonly Dictionary<string, (string AccessToken, string RefreshToken, DateTime Expiry)> Tokens = new();
        private const string Key = "default";  // Chave fixa para credenciais hardcoded

        public static void StoreTokens(string access, string refresh, DateTime expiry)
        {
            Tokens[Key] = (access, refresh, expiry);
        }

        public static (string Access, string Refresh, DateTime Expiry)? GetTokens()
        {
            return Tokens.TryGetValue(Key, out var token) ? token : null;
        }

        public static void ClearTokens()
        {
            Tokens.Remove(Key);
        }
    }
}