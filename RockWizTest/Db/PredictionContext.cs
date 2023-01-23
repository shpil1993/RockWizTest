using Microsoft.EntityFrameworkCore;
using RockWizTest.Model;

namespace RockWizTest.Db
{
    public class PredictionDbContext : DbContext
    {
        #region DbSets

        public DbSet<Word> Words { get; set; }

        #endregion

        public PredictionDbContext(DbContextOptions<PredictionDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
