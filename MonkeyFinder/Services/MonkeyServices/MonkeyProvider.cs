using MonkeyFinder.MonkeyAPI;
using MonkeyFinder.Services.MonkeyServices;

namespace MonkeyFinder.Services
{
    public class MonkeyService : IMonkeyService
    {
        // URI to get the list of monkeys
        private const string _uriMonkeyData = "app-monkeys/master/MonkeysApp/monkeydata.json";

        // HttpClient to get the list of monkeys
        private readonly MonkeyHttpClient _monkeyHttpClient;

        // List of monkeys
        private List<Monkey> _monkeysList; 

        public MonkeyService(MonkeyHttpClient monkeyHttpClient)
        {
            _monkeyHttpClient = monkeyHttpClient ?? throw new ArgumentNullException(nameof(monkeyHttpClient));
            _monkeysList = new List<Monkey>();
        }

        /// <summary>
        /// Get the list of monkeys from the API
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<List<Monkey>> GetMonkeysAsync()
        {
            if(_monkeysList?.Count > 0)
            {
                return _monkeysList;
            }

            // Get the list of monkeys from the API
            _monkeysList = await _monkeyHttpClient
                           .GetListItemsByUriAsync<Monkey>(_uriMonkeyData)
                           .ConfigureAwait(false);

            return _monkeysList;
        }
    }
}
