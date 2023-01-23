using Microsoft.EntityFrameworkCore;
using RockWizTest.Db;
using RockWizTest.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockWizTest.Services
{
    public class CustomWordPredictionService : ICustomWordPredictionService
    {
        #region Fields

        private readonly PredictionDbContext _predictionDbContext;

        #endregion

        public CustomWordPredictionService(PredictionDbContext predictionDbContext)
        {
            _predictionDbContext = predictionDbContext;
        }

        #region Methods

        public async Task<List<Word>> GetPredictions(string text)
        {
            return await _predictionDbContext.Words.Where(x => x.Value!.ToLower().StartsWith(text)).ToListAsync();
        }

        #endregion
    }
}
