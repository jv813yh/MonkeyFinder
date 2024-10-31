using Newtonsoft.Json;

namespace MonkeyFinder.MonkeyAPI
{
    public class MonkeyHttpClient
    {
        private readonly HttpClient _httpClient;
        public MonkeyHttpClient(HttpClient httpClient) 
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Get the list of items from the API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<List<T>> GetListItemsByUriAsync<T>(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }

            List<T> returnList = new List<T>();

            try
            {
                HttpResponseMessage response = await _httpClient
                                                .GetAsync(uri)
                                                .ConfigureAwait(false);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response
                    string content = await response
                                    .Content    
                                    .ReadAsStringAsync()    
                                    .ConfigureAwait(false);

                    // Deserialize the content to a list of items
                    returnList = JsonConvert
                                .DeserializeObject<List<T>>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return returnList;
        }
    }
}
