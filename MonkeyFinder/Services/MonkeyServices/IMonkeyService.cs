namespace MonkeyFinder.Services.MonkeyServices
{
    public interface IMonkeyService
    {
        Task<List<Monkey>> GetMonkeysAsync();
    }
}
