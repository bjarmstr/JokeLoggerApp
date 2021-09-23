using JokeLogger.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokeLogger.Repos.Repository
{
    public class LogJokeRepository : ILogJokeRepository
    {
        private readonly LogJokeContext _context;
        public LogJokeRepository(LogJokeContext context)
        {
            _context = context;
        }
        public async Task<LogJoke> Create(LogJoke logJoke)
        {
            _context.LogJokes.Add(logJoke);
            await _context.SaveChangesAsync();
            return logJoke;
        }

        public async Task<IEnumerable<LogJoke>> Get()
        {
            return await _context.LogJokes.ToListAsync();
        }

        public async Task<LogJoke> Get(int id)
        {
            return await _context.LogJokes.FindAsync(id);
        }

        public async Task<IEnumerable<LogJoke>> GetCount(int count)
        {
            //would this be considered business logic & belongs somewhere else?
            return await _context.LogJokes.Skip(Math.Max(0, _context.LogJokes.Count() - count)).ToListAsync();




        }
    }
}
