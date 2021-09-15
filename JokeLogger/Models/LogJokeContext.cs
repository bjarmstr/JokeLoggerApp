using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokeLogger.Models
{
    public class LogJokeContext: DbContext
    {
        public LogJokeContext(DbContextOptions<LogJokeContext> options)
            : base(options) {
            Database.EnsureCreated();
        
        }
        public DbSet<LogJoke> LogJokes { get; set; }
    }
}
