using JokeLogger.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JokeLogger.Repos

{
    public class LogJokeContext : DbContext
    {
        public LogJokeContext(DbContextOptions<LogJokeContext> options)
            : base(options)
        {
            Database.EnsureCreated();

        }
        public DbSet<LogJoke> LogJokes { get; set; }
    }
}
