using RockWizTest.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RockWizTest.Services
{
    public interface ICustomWordPredictionService
    {

        #region Methods

        Task<List<Word>> GetPredictions(string text);

        #endregion
    }
}
