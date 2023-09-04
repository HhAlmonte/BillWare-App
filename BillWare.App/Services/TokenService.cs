namespace BillWare.App.Services
{
    public class TokenService
    {
        private const string TokenKey = "access_token";
        private readonly LocalStorageService _localStorageService;

        public TokenService(LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorageService.GetItem(TokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorageService.SetItem(TokenKey, token);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorageService.RemoveItem(TokenKey);
        }
    }
}
