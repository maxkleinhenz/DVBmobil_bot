using DVB_Bot.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace DVB_Bot.Data.EfModel
{
    public class DvbBotContext : DbContext
    {
        public DbSet<Stop> Stops { get; set; }
        public DbSet<FavoriteStop> FavoriteStops { get; set; }

        private readonly string _connectionString;

        internal DvbBotContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
