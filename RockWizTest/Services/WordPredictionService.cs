using Newtonsoft.Json;
using RockWizTest.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RockWizTest.Services
{
    public class WordPredictionService : IWordPredictionService
    {
        #region Fields

        private readonly HttpClient _httpClient;

        #endregion

        public WordPredictionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Methods

        public async Task<IEnumerable<Word>?> GetWordPrediction(string lang, string sentence)
        {
            var query = $"misc/getPredictions?locale={lang}&text={sentence}";

            var resp = await _httpClient.GetAsync(query);

            if (resp.IsSuccessStatusCode)
            {
                var str = await resp.Content.ReadAsStringAsync();

                var words = JsonConvert.DeserializeObject<List<string>>(str);

                return words?.Select(x => new Word() { Value = x }).ToList();
            }

            return null;
        }

        #endregion
    }
}
