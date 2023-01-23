using RockWizTest.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RockWizTest.Services
{
    public interface IWordPredictionService
    {
        #region Methods

        Task<IEnumerable<Word>?> GetWordPrediction(string lang, string sentence);

        #endregion
    }
}